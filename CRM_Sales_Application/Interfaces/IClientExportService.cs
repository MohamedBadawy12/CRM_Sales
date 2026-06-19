using CRM_Sales_Application.DTOs;

namespace CRM_Sales_Application.Interfaces
{
    public interface IClientExportService
    {
        byte[] ExportToExcel(IEnumerable<ClientDto> clients);
        byte[] ExportToPdf(IEnumerable<ClientDto> clients);
    }
}
