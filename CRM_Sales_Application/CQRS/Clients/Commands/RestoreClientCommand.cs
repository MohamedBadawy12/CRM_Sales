using MediatR;

namespace CRM_Sales_Application.CQRS.Clients.Commands
{
    public record RestoreClientCommand(Guid Id) : IRequest<bool>;
}
