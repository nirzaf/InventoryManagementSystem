using InventoryManagementSystem.Core.Models;
using MediatR;

namespace InventoryManagementSystem.Core.Features.Items.Queries;

public record ForecastDemandQuery(int ItemId, int HorizonDays = 30) : IRequest<DemandForecastResult>;

public record ForecastAllItemsDemandQuery(int HorizonDays = 30) : IRequest<IReadOnlyList<DemandForecastResult>>;
