using CRM_Sales_Application.DTOs;
using CRM_Sales_Application.Interfaces;

namespace CRM_Sales_Infrastructure.ExportServices
{
    public class ClientExportService : IClientExportService
    {
        private readonly ClientExcelExportService _excelService;
        private readonly ClientPdfExportService _pdfService;

        public ClientExportService()
        {
            _excelService = new ClientExcelExportService();
            _pdfService = new ClientPdfExportService();
        }

        public byte[] ExportToExcel(IEnumerable<ClientDto> clients)
            => _excelService.ExportToExcel(clients);

        public byte[] ExportToPdf(IEnumerable<ClientDto> clients)
            => _pdfService.ExportToPdf(clients);
    }
}
