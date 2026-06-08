using InventoryManagementSystem.Core.Entities;
using InventoryManagementSystem.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryManagementSystem.Core.Features.Items.Queries;

public class SearchItemsQueryHandler : IRequestHandler<SearchItemsQuery, IEnumerable<Item>>
{
    private readonly IItemService _itemService;
    private readonly ILogger<SearchItemsQueryHandler> _logger;

    public SearchItemsQueryHandler(IItemService itemService, ILogger<SearchItemsQueryHandler> logger)
    {
        _itemService = itemService;
        _logger = logger;
    }

    public async Task<IEnumerable<Item>> Handle(SearchItemsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Handling SearchItemsQuery for term={Term}", request.SearchTerm);
        return await _itemService.SearchAsync(request.SearchTerm);
    }
}
