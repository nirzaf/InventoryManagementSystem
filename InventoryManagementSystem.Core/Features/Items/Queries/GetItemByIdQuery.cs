using InventoryManagementSystem.Core.Entities;
using MediatR;

namespace InventoryManagementSystem.Core.Features.Items.Queries;

public record GetItemByIdQuery(int Id) : IRequest<Item?>;
