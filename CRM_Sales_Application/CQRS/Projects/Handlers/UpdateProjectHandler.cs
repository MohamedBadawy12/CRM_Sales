using AutoMapper;
using CRM_Sales_Application.CQRS.Projects.Commands;
using CRM_Sales_Application.DTOs;
using CRM_Sales_Core.Interfaces;
using MediatR;

namespace CRM_Sales_Application.CQRS.Projects.Handlers
{
    public class UpdateProjectHandler : IRequestHandler<UpdateProjectCommand, ProjectDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateProjectHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProjectDto> Handle(
            UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(request.Id);
            project.Update(request.ProjectName, request.Location, request.Area);
            await _unitOfWork.Projects.UpdateAsync(project);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ProjectDto>(project);
        }
    }
}
