using InventoryManagementSystem.Core.Entities;
using MediatR;

namespace InventoryManagementSystem.Core.Features.Items.Commands;

public record CreateItemCommand(string ItemCode, string Description, decimal Rate, int? SupplierId) : IRequest<Item>;
