using InventoryManagementSystem.Core.Interfaces;
using InventoryManagementSystem.Core.Models;
using MediatR;

namespace InventoryManagementSystem.Core.Features.Stock.Queries;

public class DetectAnomaliesHandler : IRequestHandler<DetectAnomaliesQuery, IReadOnlyList<StockAnomaly>>
{
    private readonly IAnomalyDetectionService _service;

    public DetectAnomaliesHandler(IAnomalyDetectionService service) => _service = service;

    public async Task<IReadOnlyList<StockAnomaly>> Handle(DetectAnomaliesQuery request, CancellationToken ct)
        => await _service.DetectAnomaliesAsync(request.From, request.To);
}
