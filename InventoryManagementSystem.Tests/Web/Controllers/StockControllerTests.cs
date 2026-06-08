using AutoFixture;
using FluentAssertions;
using InventoryManagementSystem.Core.Entities;
using InventoryManagementSystem.Core.Interfaces;
using InventoryManagementSystem.Tests.Common;
using InventoryManagementSystem.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace InventoryManagementSystem.Tests.Web.Controllers;

public class StockControllerTests
{
    private readonly Fixture _fixture = InventoryFixtureFactory.Create();
    private readonly Mock<IStockService> _stockServiceMock = new();
    private readonly Mock<IItemService> _itemServiceMock = new();
    private readonly Mock<ILocationService> _locationServiceMock = new();
    private readonly StockController _sut;

    public StockControllerTests()
    {
        _sut = new StockController(
            _stockServiceMock.Object,
            _itemServiceMock.Object,
            _locationServiceMock.Object);
    }

    [Fact]
    public async Task Index_WhenCalled_ReturnsViewWithStocks()
    {
        // Arrange
        var stocks = _fixture.CreateMany<StockInHand>(3);
        _stockServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(stocks);

        // Act
        var result = await _sut.Index();

        // Assert
        result.Should().BeOfType<ViewResult>();
        var viewResult = (ViewResult)result;
        viewResult.Model.Should().BeEquivalentTo(stocks);
        _stockServiceMock.Verify(s => s.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task ReceiveGet_WhenCalled_PopulatesViewBagItemsAndLocations()
    {
        // Arrange
        var items = _fixture.CreateMany<Item>(2);
        var locations = _fixture.CreateMany<Location>(2);
        _itemServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(items);
        _locationServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(locations);

        // Act
        var result = await _sut.Receive();

        // Assert
        result.Should().BeOfType<ViewResult>();
        _sut.ViewBag.Items.Should().BeEquivalentTo(items);
        _sut.ViewBag.Locations.Should().BeEquivalentTo(locations);
    }

    [Fact]
    public async Task ReceivePost_WhenServiceSucceeds_SetsSuccessTempDataAndRedirectsToIndex()
    {
        // Arrange
        var itemId = _fixture.Create<int>();
        var locationId = _fixture.Create<int>();
        var quantity = 5;
        var notes = "test";
        _stockServiceMock.Setup(s => s.ReceiveStockAsync(itemId, locationId, quantity, notes))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _sut.Receive(itemId, locationId, quantity, notes);

        // Assert
        result.Should().BeOfType<RedirectToActionResult>();
        var redirect = (RedirectToActionResult)result;
        redirect.ActionName.Should().Be(nameof(StockController.Index));
        _sut.TempData["Success"].Should().NotBeNull();
        _sut.TempData["Error"].Should().BeNull();
        _stockServiceMock.Verify(s => s.ReceiveStockAsync(itemId, locationId, quantity, notes), Times.Once);
    }

    [Fact]
    public async Task ReceivePost_WhenServiceThrows_CapturesErrorInTempData()
    {
        // Arrange
        var itemId = _fixture.Create<int>();
        var locationId = _fixture.Create<int>();
        _stockServiceMock.Setup(s => s.ReceiveStockAsync(itemId, locationId, -1, null))
            .ThrowsAsync(new ArgumentException("Quantity must be positive"));

        // Act
        var result = await _sut.Receive(itemId, locationId, -1, null);

        // Assert
        result.Should().BeOfType<RedirectToActionResult>();
        _sut.TempData["Error"].Should().Be("Quantity must be positive");
        _sut.TempData["Success"].Should().BeNull();
    }

    [Fact]
    public async Task TransferPost_WhenQuantityIsNegative_SetsErrorTempData()
    {
        // Arrange
        _stockServiceMock.Setup(s => s.TransferStockAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), -1, It.IsAny<string?>()))
            .ThrowsAsync(new ArgumentException("Quantity must be positive"));

        // Act
        var result = await _sut.Transfer(1, 2, 3, -1, null);

        // Assert
        result.Should().BeOfType<RedirectToActionResult>();
        _sut.TempData["Error"].Should().Be("Quantity must be positive");
    }

    [Fact]
    public async Task TransferPost_WhenSourceEqualsDestination_SetsErrorTempData()
    {
        // Arrange
        _stockServiceMock.Setup(s => s.TransferStockAsync(1, 1, 1, 5, null))
            .ThrowsAsync(new ArgumentException("Source and destination must be different"));

        // Act
        var result = await _sut.Transfer(1, 1, 1, 5, null);

        // Assert
        _sut.TempData["Error"].Should().Be("Source and destination must be different");
    }

    [Fact]
    public async Task TransferPost_WhenInsufficientStock_SetsErrorTempData()
    {
        // Arrange
        _stockServiceMock.Setup(s => s.TransferStockAsync(1, 1, 2, 100, null))
            .ThrowsAsync(new InvalidOperationException("Insufficient stock at source location"));

        // Act
        var result = await _sut.Transfer(1, 1, 2, 100, null);

        // Assert
        _sut.TempData["Error"].Should().Be("Insufficient stock at source location");
    }

    [Fact]
    public async Task SellPost_WhenInsufficientStock_SetsErrorTempData()
    {
        // Arrange
        _stockServiceMock.Setup(s => s.SellStockAsync(1, 1, 99, null))
            .ThrowsAsync(new InvalidOperationException("Insufficient stock for sale"));

        // Act
        var result = await _sut.Sell(1, 1, 99, null);

        // Assert
        _sut.TempData["Error"].Should().Be("Insufficient stock for sale");
    }

    [Fact]
    public async Task SellPost_WhenSucceeds_SetsSuccessTempData()
    {
        // Arrange
        _stockServiceMock.Setup(s => s.SellStockAsync(1, 1, 2, "ok"))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _sut.Sell(1, 1, 2, "ok");

        // Assert
        result.Should().BeOfType<RedirectToActionResult>();
        _sut.TempData["Success"].Should().NotBeNull();
    }

    [Fact]
    public async Task Transactions_WhenCalled_ReturnsViewWithFilteredTransactions()
    {
        // Arrange
        var from = DateTime.UtcNow.AddDays(-7);
        var to = DateTime.UtcNow;
        var transactions = _fixture.CreateMany<StockTransaction>(4);
        _stockServiceMock.Setup(s => s.GetTransactionsAsync(from, to)).ReturnsAsync(transactions);

        // Act
        var result = await _sut.Transactions(from, to);

        // Assert
        result.Should().BeOfType<ViewResult>();
        ((ViewResult)result).Model.Should().BeEquivalentTo(transactions);
        _stockServiceMock.Verify(s => s.GetTransactionsAsync(from, to), Times.Once);
    }
}
