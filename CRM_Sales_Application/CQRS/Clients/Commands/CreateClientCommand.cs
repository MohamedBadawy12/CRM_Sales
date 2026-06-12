using CRM_Sales_Application.DTOs;
using MediatR;

namespace CRM_Sales_Application.CQRS.Clients.Commands
{
    public record CreateClientCommand(
        string ClientName,
        string Phone,
        Guid ProjectId,
        string Type,
        Guid AgentId,
        Guid? PreviousAgentId) : IRequest<ClientDto>;
}
