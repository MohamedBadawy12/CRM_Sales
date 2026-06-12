using CRM_Sales_Application.CQRS.SalesAgents.Commands;
using CRM_Sales_Core.Interfaces;
using MediatR;

namespace CRM_Sales_Application.CQRS.SalesAgents.Handlers
{
    public class DeleteSalesAgentHandler : IRequestHandler<DeleteSalesAgentCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteSalesAgentHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            DeleteSalesAgentCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.SalesAgents.DeleteAsync(request.Id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
