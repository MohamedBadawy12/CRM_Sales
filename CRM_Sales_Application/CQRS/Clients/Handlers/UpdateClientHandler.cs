using AutoMapper;
using CRM_Sales_Application.CQRS.Clients.Commands;
using CRM_Sales_Application.DTOs;
using CRM_Sales_Core.Interfaces;
using MediatR;

namespace CRM_Sales_Application.CQRS.Clients.Handlers
{
    public class UpdateClientHandler : IRequestHandler<UpdateClientCommand, ClientDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateClientHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ClientDto> Handle(
            UpdateClientCommand request, CancellationToken cancellationToken)
        {
            var client = await _unitOfWork.Clients.GetByIdAsync(request.Id);
            client.Update(
                request.ClientName,
                request.Phone,
                request.ProjectId,
                request.Type,
                request.AgentId,
                request.PreviousAgentId);

            await _unitOfWork.Clients.UpdateAsync(client);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ClientDto>(client);
        }
    }
}
