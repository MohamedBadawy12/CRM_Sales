using MediatR;

namespace CRM_Sales_Application.CQRS.Teams.Commands
{
    public record DeleteTeamCommand(Guid Id) : IRequest<bool>;
}
