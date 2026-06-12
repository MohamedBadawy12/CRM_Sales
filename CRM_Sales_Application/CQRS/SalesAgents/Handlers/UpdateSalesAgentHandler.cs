using AutoMapper;
using CRM_Sales_Application.CQRS.SalesAgents.Commands;
using CRM_Sales_Application.DTOs;
using CRM_Sales_Core.Interfaces;
using MediatR;

namespace CRM_Sales_Application.CQRS.SalesAgents.Handlers
{
    public class UpdateSalesAgentHandler : IRequestHandler<UpdateSalesAgentCommand, SalesAgentDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateSalesAgentHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SalesAgentDto> Handle(
            UpdateSalesAgentCommand request, CancellationToken cancellationToken)
        {
            var agent = await _unitOfWork.SalesAgents.GetByIdAsync(request.Id);
            agent.Update(request.AgentName, request.Role, request.TeamId, request.LeaderId);
            await _unitOfWork.SalesAgents.UpdateAsync(agent);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<SalesAgentDto>(agent);
        }
    }
}
