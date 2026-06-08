using InventoryManagementSystem.Core.Entities;
using MediatR;

namespace InventoryManagementSystem.Core.Features.Stock.Queries;

public record GetAllStockQuery : IRequest<IEnumerable<StockInHand>>;
