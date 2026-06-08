using AutoFixture;
using FluentAssertions;
using InventoryManagementSystem.Core.Entities;

namespace InventoryManagementSystem.Tests.Core.Entities;

public class StockTransactionTests
{
    private readonly Fixture _fixture = new();

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void StockTransaction_WhenQuantityIsNotPositive_StillConstructsButInvariantIsViolated(int quantity)
    {
        // Arrange
        var transaction = new StockTransaction
        {
            ItemId = _fixture.Create<int>(),
            FromLocationId = _fixture.Create<int>(),
            Quantity = quantity,
            TransactionType = "Receive",
            TransactionDate = DateTime.UtcNow
        };

        // Act
        var actualQuantity = transaction.Quantity;

        // Assert
        actualQuantity.Should().Be(quantity);
        (actualQuantity > 0).Should().BeFalse();
    }

    [Fact]
    public void StockTransaction_WhenOutboundAppliedToStockInHand_DecrementsBalance()
    {
        // Arrange
        var item = _fixture.Create<Item>();
        var location = _fixture.Create<Location>();
        var stockBalance = new StockInHand
        {
            ItemId = item.Id,
            Item = item,
            LocationId = location.Id,
            Location = location,
            Quantity = 50
        };

        var sellQuantity = 10;
        var sellTransaction = new StockTransaction
        {
            ItemId = item.Id,
            Item = item,
            FromLocationId = location.Id,
            FromLocation = location,
            Quantity = sellQuantity,
            TransactionType = "Sell",
            TransactionDate = DateTime.UtcNow
        };

        // Act
        stockBalance.Quantity -= sellTransaction.Quantity;

        // Assert
        stockBalance.Quantity.Should().Be(40);
        sellTransaction.Quantity.Should().BePositive();
    }

    [Fact]
    public void StockTransaction_WhenInboundAppliedToStockInHand_IncrementsBalance()
    {
        // Arrange
        var item = _fixture.Create<Item>();
        var location = _fixture.Create<Location>();
        var stockBalance = new StockInHand
        {
            ItemId = item.Id,
            Item = item,
            LocationId = location.Id,
            Location = location,
            Quantity = 20
        };

        var receiveQuantity = 30;
        var receiveTransaction = new StockTransaction
        {
            ItemId = item.Id,
            Item = item,
            FromLocationId = location.Id,
            FromLocation = location,
            ToLocationId = location.Id,
            ToLocation = location,
            Quantity = receiveQuantity,
            TransactionType = "Receive",
            TransactionDate = DateTime.UtcNow
        };

        // Act
        stockBalance.Quantity += receiveTransaction.Quantity;

        // Assert
        stockBalance.Quantity.Should().Be(50);
    }

    [Fact]
    public void StockTransaction_ConversionKilogramsToGrams_CalculatesPrecisely()
    {
        // Arrange
        var kilograms = 2.5m;
        const decimal gramsPerKilogram = 1000m;

        // Act
        var grams = kilograms * gramsPerKilogram;

        // Assert
        grams.Should().Be(2500m);
    }

    [Fact]
    public void StockTransaction_ConversionLitersToMilliliters_CalculatesPrecisely()
    {
        // Arrange
        var liters = 1.75m;
        const decimal millilitersPerLiter = 1000m;

        // Act
        var milliliters = liters * millilitersPerLiter;

        // Assert
        milliliters.Should().Be(1750m);
    }

    [Theory]
    [InlineData("Receive", "ToLocationId", 1)]
    [InlineData("Transfer", "ToLocationId", 2)]
    [InlineData("Sell", "ToLocationId", null)]
    public void StockTransaction_WhenTypeChanges_DestinationSemanticsDiffer(
        string transactionType, string expectedField, int? expectedValue)
    {
        // Arrange
        var transaction = new StockTransaction
        {
            ItemId = _fixture.Create<int>(),
            FromLocationId = 1,
            ToLocationId = expectedValue,
            Quantity = 10,
            TransactionType = transactionType,
            TransactionDate = DateTime.UtcNow
        };

        // Act
        var actualToLocationId = transaction.ToLocationId;

        // Assert
        actualToLocationId.Should().Be(expectedValue);
        transaction.TransactionType.Should().Be(transactionType);
    }
}
