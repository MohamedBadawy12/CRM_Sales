using AutoMapper;
using CRM_Sales_Application.CQRS.Teams.Queries;
using CRM_Sales_Application.DTOs;
using CRM_Sales_Core.Interfaces;
using MediatR;

namespace CRM_Sales_Application.CQRS.Teams.Handlers
{
    public class GetAllTeamsHandler : IRequestHandler<GetAllTeamsQuery, IEnumerable<TeamDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllTeamsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TeamDto>> Handle(
            GetAllTeamsQuery request, CancellationToken cancellationToken)
        {
            var teams = await _unitOfWork.Teams.GetAllAsync();
            return _mapper.Map<IEnumerable<TeamDto>>(teams);
        }
    }
}
