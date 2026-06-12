using CRM_Sales_Application.DTOs;
using MediatR;

namespace CRM_Sales_Application.CQRS.SalesAgents.Commands
{
    public record UpdateSalesAgentCommand(
        Guid Id,
        string AgentName,
        string Role,
        Guid TeamId,
        Guid? LeaderId) : IRequest<SalesAgentDto>;
}
