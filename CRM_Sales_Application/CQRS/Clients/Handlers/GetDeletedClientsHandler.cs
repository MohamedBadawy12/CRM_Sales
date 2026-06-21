using AutoMapper;
using CRM_Sales_Application.CQRS.Clients.Queries;
using CRM_Sales_Application.DTOs;
using CRM_Sales_Core.Interfaces;
using MediatR;

namespace CRM_Sales_Application.CQRS.Clients.Handlers
{
    public class GetDeletedClientsHandler
        : IRequestHandler<GetDeletedClientsQuery, IEnumerable<ClientDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDeletedClientsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClientDto>> Handle(
            GetDeletedClientsQuery request, CancellationToken cancellationToken)
        {
            var deleted = await _unitOfWork.Clients.GetAllDeletedAsync();
            return _mapper.Map<IEnumerable<ClientDto>>(deleted);
        }
    }
}