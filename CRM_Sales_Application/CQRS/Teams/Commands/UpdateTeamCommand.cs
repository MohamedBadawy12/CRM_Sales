using CRM_Sales_Application.DTOs;
using CRM_Sales_Core.Enums;
using MediatR;

namespace CRM_Sales_Application.CQRS.Teams.Commands
{
    public record UpdateTeamCommand(Guid Id, string TeamName, Floor Floor) : IRequest<TeamDto>;
}

