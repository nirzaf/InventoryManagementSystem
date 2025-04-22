using MediatR;

namespace InventoryManagementSystem.Core.Features.Items.Commands;

public record DeleteItemCommand(int Id) : IRequest;
