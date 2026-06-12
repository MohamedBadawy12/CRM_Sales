using AutoMapper;
using CRM_Sales_Application.CQRS.SalesAgents.Queries;
using CRM_Sales_Application.DTOs;
using CRM_Sales_Core.Interfaces;
using MediatR;

namespace CRM_Sales_Application.CQRS.SalesAgents.Handlers
{
    public class GetAllSalesAgentsHandler : IRequestHandler<GetAllSalesAgentsQuery, IEnumerable<SalesAgentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllSalesAgentsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SalesAgentDto>> Handle(
            GetAllSalesAgentsQuery request, CancellationToken cancellationToken)
        {
            var agents = await _unitOfWork.SalesAgents.GetAllAsync();
            return _mapper.Map<IEnumerable<SalesAgentDto>>(agents);
        }
    }
}
