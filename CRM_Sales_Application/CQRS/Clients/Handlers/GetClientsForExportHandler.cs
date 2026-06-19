using AutoMapper;
using CRM_Sales_Application.CQRS.Clients.Queries;
using CRM_Sales_Application.DTOs;
using CRM_Sales_Core.Interfaces;
using MediatR;

namespace CRM_Sales_Application.CQRS.Clients.Handlers
{
    public class GetClientsForExportHandler : IRequestHandler<GetClientsForExportQuery, IEnumerable<ClientDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetClientsForExportHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClientDto>> Handle(
           GetClientsForExportQuery request, CancellationToken cancellationToken)
        {
            var clients = await _unitOfWork.Clients.GetAllWithIncludesAsync();
            var dtos = _mapper.Map<IEnumerable<ClientDto>>(clients).ToList();

            if (!string.IsNullOrEmpty(request.Ids))
            {
                var idList = request.Ids.Split(',')
                    .Where(x => Guid.TryParse(x, out _))
                    .Select(Guid.Parse)
                    .ToHashSet();

                return dtos.Where(c => idList.Contains(c.Id))
                    .OrderByDescending(c => c.CreatedAt);
            }

            var filtered = request.Type switch
            {
                "Walk" => dtos.Where(c => c.Type == "Walk"),
                "Follow" => dtos.Where(c => c.Type == "Follow"),
                _ => dtos.AsEnumerable()
            };

            if (!string.IsNullOrEmpty(request.Search))
            {
                filtered = filtered.Where(c =>
                    c.ClientName.Contains(request.Search, StringComparison.OrdinalIgnoreCase) ||
                    c.Phone.Contains(request.Search, StringComparison.OrdinalIgnoreCase) ||
                    (c.ProjectName?.Contains(request.Search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (c.AgentName?.Contains(request.Search, StringComparison.OrdinalIgnoreCase) ?? false)
                );
            }

            if (request.Month.HasValue)
                filtered = filtered.Where(c => c.CreatedAt.Month == request.Month.Value);

            if (request.Year.HasValue)
                filtered = filtered.Where(c => c.CreatedAt.Year == request.Year.Value);

            return filtered.OrderByDescending(c => c.CreatedAt);
        }
    }
}
