using CRM_Sales_Application.DTOs;
using MediatR;

namespace CRM_Sales_Application.CQRS.Teams.Queries
{
    public record GetAllTeamsQuery() : IRequest<IEnumerable<TeamDto>>;
}
