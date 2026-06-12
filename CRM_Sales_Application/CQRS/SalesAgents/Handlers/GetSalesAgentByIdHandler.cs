using AutoMapper;
using CRM_Sales_Application.CQRS.SalesAgents.Queries;
using CRM_Sales_Application.DTOs;
using CRM_Sales_Core.Interfaces;
using MediatR;

namespace CRM_Sales_Application.CQRS.SalesAgents.Handlers
{
    public class GetSalesAgentByIdHandler : IRequestHandler<GetSalesAgentByIdQuery, SalesAgentDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetSalesAgentByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SalesAgentDto> Handle(
            GetSalesAgentByIdQuery request, CancellationToken cancellationToken)
        {
            var agent = await _unitOfWork.SalesAgents.GetByIdAsync(request.Id);
            return _mapper.Map<SalesAgentDto>(agent);
        }
    }
}
