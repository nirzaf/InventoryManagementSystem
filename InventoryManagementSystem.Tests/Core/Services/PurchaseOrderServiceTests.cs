using AutoFixture;
using FluentAssertions;
using InventoryManagementSystem.Core.Entities;
using InventoryManagementSystem.Core.Interfaces;
using InventoryManagementSystem.Core.Services;
using InventoryManagementSystem.Tests.Common;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace InventoryManagementSystem.Tests.Core.Services;

public class PurchaseOrderServiceTests
{
    private readonly Fixture _fixture = InventoryFixtureFactory.Create();
    private readonly Mock<IRepository<PurchaseOrder>> _poRepoMock = new();
    private readonly Mock<IAgenticProcurementClient> _agenticClientMock = new();
    private readonly PurchaseOrderService _sut;

    public PurchaseOrderServiceTests()
    {
        _sut = new PurchaseOrderService(_poRepoMock.Object, NullLogger<PurchaseOrderService>.Instance);
    }

    [Fact]
    public async Task CreatePurchaseOrderAsync_WhenDetailsAreValid_AddsPurchaseOrderWithPendingStatus()
    {
        // Arrange
        var po = _fixture.Create<PurchaseOrder>();
        po.Status = null;
        po.TotalAmount = 0m;

        var details = new List<OrderDetail>
        {
            new() { ItemId = 1, Quantity = 5, UnitPrice = 10.00m },
            new() { ItemId = 2, Quantity = 3, UnitPrice = 20.00m }
        };

        var expectedTotal = details.Sum(d => d.Quantity * d.UnitPrice);
        _poRepoMock.Setup(r => r.AddAsync(It.IsAny<PurchaseOrder>()))
            .ReturnsAsync((PurchaseOrder p) => p);

        // Act
        var result = await _sut.CreateAsync(po, details);

        // Assert
        result.Status.Should().Be("Pending");
        result.TotalAmount.Should().Be(expectedTotal);
        result.OrderDetails.Should().BeEquivalentTo(details);
        _poRepoMock.Verify(r => r.AddAsync(It.Is<PurchaseOrder>(p =>
            p.Status == "Pending" &&
            p.TotalAmount == expectedTotal &&
            p.OrderDetails.Count == details.Count)), Times.Once);
    }

    [Fact]
    public async Task CreatePurchaseOrderAsync_WhenCalled_InvokesAddAsyncExactlyOnce()
    {
        // Arrange
        var po = _fixture.Create<PurchaseOrder>();
        var details = new List<OrderDetail>
        {
            new() { ItemId = 1, Quantity = 1, UnitPrice = 1.00m }
        };
        _poRepoMock.Setup(r => r.AddAsync(It.IsAny<PurchaseOrder>()))
            .ReturnsAsync((PurchaseOrder p) => p);

        // Act
        await _sut.CreateAsync(po, details);

        // Assert
        _poRepoMock.Verify(r => r.AddAsync(It.IsAny<PurchaseOrder>()), Times.Once);
    }

    [Fact]
    public async Task CreatePurchaseOrderAsync_WhenDetailsAreEmpty_SetsTotalAmountToZero()
    {
        // Arrange
        var po = _fixture.Create<PurchaseOrder>();
        po.TotalAmount = 999m;
        _poRepoMock.Setup(r => r.AddAsync(It.IsAny<PurchaseOrder>()))
            .ReturnsAsync((PurchaseOrder p) => p);

        // Act
        var result = await _sut.CreateAsync(po, new List<OrderDetail>());

        // Assert
        result.TotalAmount.Should().Be(0m);
    }

    [Fact]
    public async Task GetByIdAsync_WhenPurchaseOrderExists_ReturnsPurchaseOrder()
    {
        // Arrange
        var po = _fixture.Create<PurchaseOrder>();
        _poRepoMock.Setup(r => r.GetByIdAsync(po.Id)).ReturnsAsync(po);

        // Act
        var result = await _sut.GetByIdAsync(po.Id);

        // Assert
        result.Should().BeEquivalentTo(po);
    }

    [Fact]
    public async Task GetByIdAsync_WhenPurchaseOrderDoesNotExist_ReturnsNull()
    {
        // Arrange
        var id = _fixture.Create<int>();
        _poRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((PurchaseOrder?)null);

        // Act
        var result = await _sut.GetByIdAsync(id);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateStatusAsync_WhenPurchaseOrderDoesNotExist_ThrowsInvalidOperationException()
    {
        // Arrange
        var id = _fixture.Create<int>();
        _poRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((PurchaseOrder?)null);

        // Act
        var act = async () => await _sut.UpdateStatusAsync(id, "Approved");

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Purchase order not found");
    }

    [Fact]
    public async Task UpdateStatusAsync_WhenSupplierIsValid_UpdatesOrderStatus()
    {
        // Arrange
        var po = _fixture.Build<PurchaseOrder>()
            .With(p => p.Status, "Pending")
            .Create();
        _poRepoMock.Setup(r => r.GetByIdAsync(po.Id)).ReturnsAsync(po);

        // Act
        await _sut.UpdateStatusAsync(po.Id, "Approved");

        // Assert
        po.Status.Should().Be("Approved");
        _poRepoMock.Verify(r => r.UpdateAsync(po), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenPurchaseOrderExists_RemovesPurchaseOrder()
    {
        // Arrange
        var po = _fixture.Create<PurchaseOrder>();
        _poRepoMock.Setup(r => r.GetByIdAsync(po.Id)).ReturnsAsync(po);

        // Act
        await _sut.DeleteAsync(po.Id);

        // Assert
        _poRepoMock.Verify(r => r.DeleteAsync(po), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenPurchaseOrderDoesNotExist_DoesNotInvokeDelete()
    {
        // Arrange
        var id = _fixture.Create<int>();
        _poRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((PurchaseOrder?)null);

        // Act
        await _sut.DeleteAsync(id);

        // Assert
        _poRepoMock.Verify(r => r.DeleteAsync(It.IsAny<PurchaseOrder>()), Times.Never);
    }

    [Fact]
    public async Task DraftPurchaseOrderFromAgentAsync_WhenAgentGeneratesSupplierDraft_PersistsPurchaseOrderInDraftState()
    {
        // Arrange
        var supplierId = _fixture.Create<int>();
        var itemIds = _fixture.CreateMany<int>(3);
        var agentDraft = new AgenticProcurementDraft
        {
            SupplierId = supplierId,
            Subject = "RFQ: Restock of WIDGET-001",
            Body = "Please quote 50 units of WIDGET-001 for delivery to Location-1.",
            GeneratedAt = DateTime.UtcNow
        };

        _agenticClientMock
            .Setup(c => c.DraftSupplierCorrespondenceAsync(supplierId, It.IsAny<IEnumerable<int>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(agentDraft);

        var details = itemIds.Select(id => new OrderDetail { ItemId = id, Quantity = 10, UnitPrice = 5.00m }).ToList();
        var draftPo = _fixture.Build<PurchaseOrder>()
            .With(p => p.SupplierId, supplierId)
            .With(p => p.Notes, $"{agentDraft.Subject}\n{agentDraft.Body}")
            .Create();
        _poRepoMock.Setup(r => r.AddAsync(It.IsAny<PurchaseOrder>()))
            .ReturnsAsync((PurchaseOrder p) => p);

        // Act
        var generated = await _agenticClientMock.Object.DraftSupplierCorrespondenceAsync(supplierId, itemIds);
        var result = await _sut.CreateAsync(draftPo, details);
        result.Status = "Draft";
        await _poRepoMock.Object.AddAsync(result);

        // Assert
        generated.Should().BeEquivalentTo(agentDraft);
        generated.Subject.Should().Contain("RFQ");
        result.Status.Should().Be("Draft");
        result.Notes.Should().Contain(agentDraft.Subject);
        result.Notes.Should().Contain(agentDraft.Body);
        _agenticClientMock.Verify(c =>
            c.DraftSupplierCorrespondenceAsync(supplierId, It.IsAny<IEnumerable<int>>(), It.IsAny<CancellationToken>()),
            Times.Once);
        _poRepoMock.Verify(r => r.AddAsync(It.Is<PurchaseOrder>(p => p.Status == "Draft")), Times.Once);
    }
}
