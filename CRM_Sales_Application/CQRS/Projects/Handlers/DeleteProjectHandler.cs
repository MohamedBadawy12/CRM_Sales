using CRM_Sales_Application.CQRS.Projects.Commands;
using CRM_Sales_Core.Interfaces;
using MediatR;

namespace CRM_Sales_Application.CQRS.Projects.Handlers
{
    public class DeleteProjectHandler : IRequestHandler<DeleteProjectCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProjectHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.Projects.DeleteAsync(request.Id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
