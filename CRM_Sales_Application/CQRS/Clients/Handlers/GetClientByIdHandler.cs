using AutoMapper;
using CRM_Sales_Application.CQRS.Clients.Queries;
using CRM_Sales_Application.DTOs;
using CRM_Sales_Core.Interfaces;
using MediatR;

namespace CRM_Sales_Application.CQRS.Clients.Handlers
{
    public class GetClientByIdHandler : IRequestHandler<GetClientByIdQuery, ClientDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetClientByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ClientDto> Handle(
            GetClientByIdQuery request, CancellationToken cancellationToken)
        {
            var client = await _unitOfWork.Clients.GetByIdAsync(request.Id);
            return _mapper.Map<ClientDto>(client);
        }
    }
}
