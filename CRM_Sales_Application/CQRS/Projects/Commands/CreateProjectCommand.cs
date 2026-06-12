using CRM_Sales_Application.DTOs;
using MediatR;

namespace CRM_Sales_Application.CQRS.Projects.Commands
{
    public record CreateProjectCommand(
        string ProjectName,
        string Location,
        string Area) : IRequest<ProjectDto>;
}
