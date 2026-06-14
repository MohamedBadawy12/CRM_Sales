using AutoMapper;
using CRM_Sales_Application.CQRS.Clients.Queries;
using CRM_Sales_Application.DTOs;
using CRM_Sales_Core.Interfaces;
using MediatR;

namespace CRM_Sales_Application.CQRS.Clients.Handlers
{
    public class GetClientsByTypeHandler : IRequestHandler<GetClientsByTypeQuery, IEnumerable<ClientDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetClientsByTypeHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClientDto>> Handle(
            GetClientsByTypeQuery request, CancellationToken cancellationToken)
        {
            var clients = await _unitOfWork.Clients.GetAllWithIncludesAsync();
            var filtered = clients.Where(c => c.Type == request.Type);
            return _mapper.Map<IEnumerable<ClientDto>>(filtered);
        }
    }
}
