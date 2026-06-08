using InventoryManagementSystem.Core.Entities;
using MediatR;

namespace InventoryManagementSystem.Core.Features.Stock.Queries;

public record GetStockTransactionsQuery(DateTime? From, DateTime? To) : IRequest<IEnumerable<StockTransaction>>;
