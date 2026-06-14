using CRM_Sales_Application.DTOs;
using MediatR;

namespace CRM_Sales_Application.CQRS.Clients.Queries
{
    public record GetClientsByTypeQuery(string Type) : IRequest<IEnumerable<ClientDto>>;
}
