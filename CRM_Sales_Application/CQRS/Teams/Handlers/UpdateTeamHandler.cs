using AutoMapper;
using CRM_Sales_Application.CQRS.Teams.Commands;
using CRM_Sales_Application.DTOs;
using CRM_Sales_Core.Interfaces;
using MediatR;

namespace CRM_Sales_Application.CQRS.Teams.Handlers
{
    public class UpdateTeamHandler : IRequestHandler<UpdateTeamCommand, TeamDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateTeamHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TeamDto> Handle(
            UpdateTeamCommand request, CancellationToken cancellationToken)
        {
            var team = await _unitOfWork.Teams.GetByIdAsync(request.Id);
            team.Update(request.TeamName, request.Floor);
            await _unitOfWork.Teams.UpdateAsync(team);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<TeamDto>(team);
        }
    }
}
