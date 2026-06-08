using InventoryManagementSystem.Core.Entities;
using InventoryManagementSystem.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryManagementSystem.Core.Features.Stock.Queries;

public class GetAllStockQueryHandler : IRequestHandler<GetAllStockQuery, IEnumerable<StockInHand>>
{
    private readonly IStockService _stockService;
    private readonly ILogger<GetAllStockQueryHandler> _logger;

    public GetAllStockQueryHandler(IStockService stockService, ILogger<GetAllStockQueryHandler> logger)
    {
        _stockService = stockService;
        _logger = logger;
    }

    public async Task<IEnumerable<StockInHand>> Handle(GetAllStockQuery request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Handling GetAllStockQuery");
        return await _stockService.GetAllAsync();
    }
}
