using InventoryManagementSystem.Core.Models;
using MediatR;

namespace InventoryManagementSystem.Core.Features.Stock.Queries;

public record DetectAnomaliesQuery(DateTime? From = null, DateTime? To = null) : IRequest<IReadOnlyList<StockAnomaly>>;
