using AutoMapper;
using CRM_Sales_Application.CQRS.SalesAgents.Queries;
using CRM_Sales_Application.DTOs;
using CRM_Sales_Core.Interfaces;
using MediatR;

namespace CRM_Sales_Application.CQRS.SalesAgents.Handlers
{
    public class GetAgentsByLeaderIdHandler : IRequestHandler<GetAgentsByLeaderIdQuery, IEnumerable<SalesAgentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAgentsByLeaderIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SalesAgentDto>> Handle(
            GetAgentsByLeaderIdQuery request, CancellationToken cancellationToken)
        {
            var agents = await _unitOfWork.SalesAgents.GetAllAsync();
            var filtered = agents.Where(a => a.LeaderId == request.LeaderId);
            return _mapper.Map<IEnumerable<SalesAgentDto>>(filtered);
        }
    }
}
