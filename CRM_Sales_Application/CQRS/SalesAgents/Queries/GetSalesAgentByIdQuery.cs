using CRM_Sales_Application.DTOs;
using MediatR;

namespace CRM_Sales_Application.CQRS.SalesAgents.Queries
{
    public record GetSalesAgentByIdQuery(Guid Id) : IRequest<SalesAgentDto>;
}
