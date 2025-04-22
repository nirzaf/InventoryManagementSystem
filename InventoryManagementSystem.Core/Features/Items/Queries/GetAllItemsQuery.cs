using InventoryManagementSystem.Core.Entities;
using MediatR;

namespace InventoryManagementSystem.Core.Features.Items.Queries;

public record GetAllItemsQuery : IRequest<IEnumerable<Item>>;
