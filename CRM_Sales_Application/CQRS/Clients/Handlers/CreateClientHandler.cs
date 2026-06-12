using AutoMapper;
using CRM_Sales_Application.CQRS.Clients.Commands;
using CRM_Sales_Application.DTOs;
using CRM_Sales_Core.Entites;
using CRM_Sales_Core.Interfaces;
using MediatR;

namespace CRM_Sales_Application.CQRS.Clients.Handlers
{
    public class CreateClientHandler : IRequestHandler<CreateClientCommand, ClientDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateClientHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ClientDto> Handle(
            CreateClientCommand request, CancellationToken cancellationToken)
        {
            var client = new Client(
                request.ClientName,
                request.Phone,
                request.ProjectId,
                request.Type,
                request.AgentId,
                request.PreviousAgentId);

            await _unitOfWork.Clients.AddAsync(client);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ClientDto>(client);
        }
    }
}
