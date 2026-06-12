using CRM_Sales_Application.DTOs;
using MediatR;

namespace CRM_Sales_Application.CQRS.Projects.Queries
{
    public record GetAllProjectsQuery() : IRequest<IEnumerable<ProjectDto>>;
}
