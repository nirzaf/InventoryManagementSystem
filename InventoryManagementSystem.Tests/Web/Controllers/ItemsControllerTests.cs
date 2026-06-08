using AutoFixture;
using FluentAssertions;
using InventoryManagementSystem.Core.Entities;
using InventoryManagementSystem.Core.Interfaces;
using InventoryManagementSystem.Tests.Common;
using InventoryManagementSystem.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;

namespace InventoryManagementSystem.Tests.Web.Controllers;

public class ItemsControllerTests
{
    private readonly Fixture _fixture = InventoryFixtureFactory.Create();
    private readonly Mock<IItemService> _itemServiceMock = new();
    private readonly Mock<ISupplierService> _supplierServiceMock = new();
    private readonly ItemsController _sut;

    public ItemsControllerTests()
    {
        _sut = new ItemsController(_itemServiceMock.Object, _supplierServiceMock.Object);

        var httpContext = new DefaultHttpContext();
        var tempDataProvider = new Mock<ITempDataProvider>();
        _sut.ControllerContext = new ControllerContext { HttpContext = httpContext };
        _sut.TempData = new TempDataDictionary(httpContext, tempDataProvider.Object);
    }

    [Fact]
    public async Task Details_WhenItemExists_ReturnsOkObjectResultWithItem()
    {
        // Arrange
        var item = _fixture.Create<Item>();
        _itemServiceMock.Setup(s => s.GetByIdAsync(item.Id)).ReturnsAsync(item);

        // Act
        var result = await _sut.Details(item.Id);

        // Assert
        result.Should().BeOfType<ViewResult>();
        var viewResult = (ViewResult)result;
        viewResult.Model.Should().BeEquivalentTo(item);
    }

    [Fact]
    public async Task Details_WhenItemDoesNotExist_ReturnsNotFoundResult()
    {
        // Arrange
        var id = _fixture.Create<int>();
        _itemServiceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((Item?)null);

        // Act
        var result = await _sut.Details(id);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
        ((NotFoundResult)result).StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task GetItemByIdOkShape_WhenServiceReturnsItem_ActionResultHasValue()
    {
        // Arrange
        var item = _fixture.Create<Item>();
        _itemServiceMock.Setup(s => s.GetByIdAsync(item.Id)).ReturnsAsync(item);

        // Act
        var result = await _sut.Details(item.Id);

        // Assert
        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        viewResult.Model.Should().BeEquivalentTo(item);
        viewResult.Model.Should().NotBeNull();
    }

    [Fact]
    public async Task Index_WhenNoSearchTerm_ReturnsPagedItems()
    {
        // Arrange
        var items = _fixture.CreateMany<Item>(5);
        _itemServiceMock.Setup(s => s.GetPagedAsync(1, 20)).ReturnsAsync(items);
        _itemServiceMock.Setup(s => s.GetCountAsync()).ReturnsAsync(items.Count);

        // Act
        var result = await _sut.Index(null, 1, 20);

        // Assert
        result.Should().BeOfType<ViewResult>();
        _itemServiceMock.Verify(s => s.GetPagedAsync(1, 20), Times.Once);
        _itemServiceMock.Verify(s => s.SearchAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Index_WhenSearchTermProvided_CallsSearchService()
    {
        // Arrange
        var search = "WIDGET";
        var items = _fixture.CreateMany<Item>(2);
        _itemServiceMock.Setup(s => s.SearchAsync(search)).ReturnsAsync(items);
        _itemServiceMock.Setup(s => s.GetCountAsync()).ReturnsAsync(items.Count);

        // Act
        var result = await _sut.Index(search, 1, 20);

        // Assert
        result.Should().BeOfType<ViewResult>();
        _itemServiceMock.Verify(s => s.SearchAsync(search), Times.Once);
        _itemServiceMock.Verify(s => s.GetPagedAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task CreateGet_WhenCalled_PopulatesSuppliersViewBag()
    {
        // Arrange
        var suppliers = _fixture.CreateMany<Supplier>(3);
        _supplierServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(suppliers);

        // Act
        var result = await _sut.Create();

        // Assert
        result.Should().BeOfType<ViewResult>();
        ((IEnumerable<Supplier>)_sut.ViewBag.Suppliers!).Should().BeEquivalentTo(suppliers);
    }

    [Fact]
    public async Task CreatePost_WhenModelIsValid_CreatesItemAndRedirects()
    {
        // Arrange
        var item = _fixture.Create<Item>();
        _itemServiceMock.Setup(s => s.CreateAsync(item)).ReturnsAsync(item);

        // Act
        var result = await _sut.Create(item);

        // Assert
        result.Should().BeOfType<RedirectToActionResult>();
        var redirect = (RedirectToActionResult)result;
        redirect.ActionName.Should().Be(nameof(ItemsController.Index));
        _itemServiceMock.Verify(s => s.CreateAsync(item), Times.Once);
    }

    [Fact]
    public async Task EditGet_WhenItemExists_PopulatesViewAndReturnsViewResult()
    {
        // Arrange
        var item = _fixture.Create<Item>();
        var suppliers = _fixture.CreateMany<Supplier>(2);
        _itemServiceMock.Setup(s => s.GetByIdAsync(item.Id)).ReturnsAsync(item);
        _supplierServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(suppliers);

        // Act
        var result = await _sut.Edit(item.Id);

        // Assert
        result.Should().BeOfType<ViewResult>();
        _sut.ViewBag.Suppliers.Should().BeEquivalentTo(suppliers);
    }

    [Fact]
    public async Task EditGet_WhenItemDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var id = _fixture.Create<int>();
        _itemServiceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((Item?)null);

        // Act
        var result = await _sut.Edit(id);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task EditPost_WhenIdMismatch_ReturnsNotFound()
    {
        // Arrange
        var item = _fixture.Build<Item>().With(i => i.Id, 1).Create();

        // Act
        var result = await _sut.Edit(99, item);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
        _itemServiceMock.Verify(s => s.UpdateAsync(It.IsAny<Item>()), Times.Never);
    }

    [Fact]
    public async Task EditPost_WhenModelIsValid_UpdatesItemAndRedirects()
    {
        // Arrange
        var item = _fixture.Create<Item>();
        _itemServiceMock.Setup(s => s.UpdateAsync(item)).Returns(Task.CompletedTask);

        // Act
        var result = await _sut.Edit(item.Id, item);

        // Assert
        result.Should().BeOfType<RedirectToActionResult>();
        _itemServiceMock.Verify(s => s.UpdateAsync(item), Times.Once);
    }

    [Fact]
    public async Task DeletePost_WhenCalled_DeletesItemAndRedirects()
    {
        // Arrange
        var id = _fixture.Create<int>();

        // Act
        var result = await _sut.Delete(id);

        // Assert
        result.Should().BeOfType<RedirectToActionResult>();
        _itemServiceMock.Verify(s => s.DeleteAsync(id), Times.Once);
    }
}
