using InventoryManagementSystem.Core.Entities;
using InventoryManagementSystem.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryManagementSystem.Core.Features.Items.Queries;

public class GetItemByIdQueryHandler : IRequestHandler<GetItemByIdQuery, Item?>
{
    private readonly IItemService _itemService;
    private readonly ILogger<GetItemByIdQueryHandler> _logger;

    public GetItemByIdQueryHandler(IItemService itemService, ILogger<GetItemByIdQueryHandler> logger)
    {
        _itemService = itemService;
        _logger = logger;
    }

    public async Task<Item?> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Handling GetItemByIdQuery for Id={Id}", request.Id);
        return await _itemService.GetByIdAsync(request.Id);
    }
}
