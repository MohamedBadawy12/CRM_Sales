using AutoMapper;
using CRM_Sales_Application.CQRS.Teams.Commands;
using CRM_Sales_Application.DTOs;
using CRM_Sales_Core.Entites;
using CRM_Sales_Core.Interfaces;
using MediatR;

namespace CRM_Sales_Application.CQRS.Teams.Handlers
{
    public class CreateTeamHandler : IRequestHandler<CreateTeamCommand, TeamDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateTeamHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TeamDto> Handle(
            CreateTeamCommand request, CancellationToken cancellationToken)
        {
            var team = new Team(request.TeamName, request.Floor);
            await _unitOfWork.Teams.AddAsync(team);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<TeamDto>(team);
        }
    }
}
