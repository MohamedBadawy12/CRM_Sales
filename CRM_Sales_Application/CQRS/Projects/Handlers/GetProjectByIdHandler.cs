using AutoMapper;
using CRM_Sales_Application.CQRS.Projects.Queries;
using CRM_Sales_Application.DTOs;
using CRM_Sales_Core.Interfaces;
using MediatR;

namespace CRM_Sales_Application.CQRS.Projects.Handlers
{
    public class GetProjectByIdHandler : IRequestHandler<GetProjectByIdQuery, ProjectDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProjectByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProjectDto> Handle(
            GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(request.Id);
            return _mapper.Map<ProjectDto>(project);
        }
    }
}
