using CRM_Sales_Application.CQRS.Clients.Commands;
using CRM_Sales_Core.Interfaces;
using MediatR;
namespace CRM_Sales_Application.CQRS.Clients.Handlers
{
    public class HardDeleteClientHandler : IRequestHandler<HardDeleteClientCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public HardDeleteClientHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            HardDeleteClientCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.Clients.HardDeleteAsync(request.Id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
