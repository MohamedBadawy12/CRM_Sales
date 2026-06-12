using MediatR;

namespace CRM_Sales_Application.CQRS.Clients.Commands
{
    public record DeleteClientCommand(Guid Id) : IRequest<bool>;
}
