using CRM_Sales_Application.DTOs;
using MediatR;

namespace CRM_Sales_Application.CQRS.Clients.Queries
{
    public record GetClientsForExportQuery(
        string? Ids,
        string Type,
        string? Search,
        int? Month,
        int? Year) : IRequest<IEnumerable<ClientDto>>;
}
