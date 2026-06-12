using AutoMapper;
using CRM_Sales_Application.CQRS.Projects.Queries;
using CRM_Sales_Application.DTOs;
using CRM_Sales_Core.Interfaces;
using MediatR;

namespace CRM_Sales_Application.CQRS.Projects.Handlers
{
    public class GetAllProjectsHandler : IRequestHandler<GetAllProjectsQuery, IEnumerable<ProjectDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllProjectsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProjectDto>> Handle(
            GetAllProjectsQuery request, CancellationToken cancellationToken)
        {
            var projects = await _unitOfWork.Projects.GetAllAsync();
            return _mapper.Map<IEnumerable<ProjectDto>>(projects);
        }
    }
}
