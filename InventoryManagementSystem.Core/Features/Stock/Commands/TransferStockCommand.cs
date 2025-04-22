using MediatR;

namespace InventoryManagementSystem.Core.Features.Stock.Commands;

public record TransferStockCommand(int ItemId, int FromLocationId, int ToLocationId, int Quantity, string? Notes) : IRequest;
