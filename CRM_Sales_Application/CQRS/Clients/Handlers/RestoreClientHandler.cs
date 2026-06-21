using CRM_Sales_Application.CQRS.Clients.Commands;
using CRM_Sales_Core.Interfaces;
using MediatR;

namespace CRM_Sales_Application.CQRS.Clients.Handlers
{
    public class RestoreClientHandler : IRequestHandler<RestoreClientCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RestoreClientHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            RestoreClientCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.Clients.RestoreAsync(request.Id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}