using MediatR;

namespace CRM_Sales_Application.CQRS.Projects.Commands
{
    public record DeleteProjectCommand(Guid Id) : IRequest<bool>;
}
