using MediatR;

namespace CRM_Sales_Application.CQRS.SalesAgents.Commands
{
    public record DeleteSalesAgentCommand(Guid Id) : IRequest<bool>;
}
