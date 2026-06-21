using MediatR;

namespace CRM_Sales_Application.CQRS.Clients.Commands
{
    public record HardDeleteClientCommand(Guid Id) : IRequest<bool>;
}
