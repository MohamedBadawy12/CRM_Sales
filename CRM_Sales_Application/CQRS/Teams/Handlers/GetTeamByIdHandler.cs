using AutoMapper;
using CRM_Sales_Application.CQRS.Teams.Queries;
using CRM_Sales_Application.DTOs;
using CRM_Sales_Core.Interfaces;
using MediatR;

namespace CRM_Sales_Application.CQRS.Teams.Handlers
{
    public class GetTeamByIdHandler : IRequestHandler<GetTeamByIdQuery, TeamDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetTeamByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TeamDto> Handle(
            GetTeamByIdQuery request, CancellationToken cancellationToken)
        {
            var team = await _unitOfWork.Teams.GetByIdAsync(request.Id);
            return _mapper.Map<TeamDto>(team);
        }
    }
}
