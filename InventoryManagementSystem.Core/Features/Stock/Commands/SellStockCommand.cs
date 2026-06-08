using MediatR;

namespace InventoryManagementSystem.Core.Features.Stock.Commands;

public record SellStockCommand(int ItemId, int LocationId, int Quantity, string? Notes) : IRequest;
