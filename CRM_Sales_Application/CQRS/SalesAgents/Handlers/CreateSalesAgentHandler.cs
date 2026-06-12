using AutoMapper;
using CRM_Sales_Application.CQRS.SalesAgents.Commands;
using CRM_Sales_Application.DTOs;
using CRM_Sales_Core.Entites;
using CRM_Sales_Core.Interfaces;
using MediatR;

namespace CRM_Sales_Application.CQRS.SalesAgents.Handlers
{
    public class CreateSalesAgentHandler : IRequestHandler<CreateSalesAgentCommand, SalesAgentDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateSalesAgentHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SalesAgentDto> Handle(
            CreateSalesAgentCommand request, CancellationToken cancellationToken)
        {
            var agent = new SalesAgent(
                request.AgentName,
                request.Role,
                request.TeamId,
                request.LeaderId);

            await _unitOfWork.SalesAgents.AddAsync(agent);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<SalesAgentDto>(agent);
        }
    }
}
