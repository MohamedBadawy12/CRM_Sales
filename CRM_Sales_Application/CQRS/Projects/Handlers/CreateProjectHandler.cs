using AutoMapper;
using CRM_Sales_Application.CQRS.Projects.Commands;
using CRM_Sales_Application.DTOs;
using CRM_Sales_Core.Entites;
using CRM_Sales_Core.Interfaces;
using MediatR;

namespace CRM_Sales_Application.CQRS.Projects.Handlers
{
    public class CreateProjectHandler : IRequestHandler<CreateProjectCommand, ProjectDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateProjectHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProjectDto> Handle(
            CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = new Project(request.ProjectName, request.Location, request.Area);
            await _unitOfWork.Projects.AddAsync(project);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ProjectDto>(project);
        }
    }
}