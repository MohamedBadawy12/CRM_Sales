using AutoMapper;
using CRM_Sales_Application.CQRS.Clients.Queries;
using CRM_Sales_Application.DTOs;
using CRM_Sales_Core.Interfaces;
using MediatR;

namespace CRM_Sales_Application.CQRS.Clients.Handlers
{
    public class GetClientsByAgentIdHandler : IRequestHandler<GetClientsByAgentIdQuery, IEnumerable<ClientDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetClientsByAgentIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClientDto>> Handle(
            GetClientsByAgentIdQuery request, CancellationToken cancellationToken)
        {
            var clients = await _unitOfWork.Clients.GetAllAsync();
            var filtered = clients.Where(c => c.AgentId == request.AgentId);
            return _mapper.Map<IEnumerable<ClientDto>>(filtered);
        }
    }
}
