using CRM_Sales_Application.CQRS.Teams.Commands;
using CRM_Sales_Core.Interfaces;
using MediatR;

namespace CRM_Sales_Application.CQRS.Teams.Handlers
{
    public class DeleteTeamHandler : IRequestHandler<DeleteTeamCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTeamHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            DeleteTeamCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.Teams.DeleteAsync(request.Id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
