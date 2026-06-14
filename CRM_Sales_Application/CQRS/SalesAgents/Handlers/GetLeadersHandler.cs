using AutoMapper;
using CRM_Sales_Application.CQRS.SalesAgents.Queries;
using CRM_Sales_Application.DTOs;
using CRM_Sales_Core.Interfaces;
using MediatR;

namespace CRM_Sales_Application.CQRS.SalesAgents.Handlers
{
    public class GetLeadersHandler : IRequestHandler<GetLeadersQuery, IEnumerable<SalesAgentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetLeadersHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SalesAgentDto>> Handle(
            GetLeadersQuery request, CancellationToken cancellationToken)
        {
            var agents = await _unitOfWork.SalesAgents.GetAllWithIncludesAsync();
            var leaders = agents.Where(a => a.Role == "TeamLeader");
            return _mapper.Map<IEnumerable<SalesAgentDto>>(leaders);
        }
    }
}
