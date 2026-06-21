using CRM_Sales_Application.DTOs;

namespace CRM_Sales_Application.Results
{
    public class ClientIndexResult
    {
        public IEnumerable<ClientDto> Clients { get; set; }
    }
}
