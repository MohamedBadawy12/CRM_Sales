using CRM_Sales_Application.CQRS.Clients.Commands;
using CRM_Sales_Core.Interfaces;
using MediatR;

namespace CRM_Sales_Application.CQRS.Clients.Handlers
{
    public class DeleteClientHandler : IRequestHandler<DeleteClientCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteClientHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            DeleteClientCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.Clients.DeleteAsync(request.Id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
