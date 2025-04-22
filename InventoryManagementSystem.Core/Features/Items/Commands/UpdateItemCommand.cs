using InventoryManagementSystem.Core.Entities;
using MediatR;

namespace InventoryManagementSystem.Core.Features.Items.Commands;

public record UpdateItemCommand(int Id, string Description, decimal Rate, int? SupplierId) : IRequest;
