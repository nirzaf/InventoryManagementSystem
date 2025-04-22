using InventoryManagementSystem.Core.Entities;
using MediatR;

namespace InventoryManagementSystem.Core.Features.Items.Queries;

public record SearchItemsQuery(string SearchTerm) : IRequest<IEnumerable<Item>>;
