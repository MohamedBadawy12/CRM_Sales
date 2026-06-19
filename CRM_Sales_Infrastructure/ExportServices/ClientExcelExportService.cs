using CRM_Sales_Application.DTOs;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace CRM_Sales_Infrastructure.ExportServices
{
    public class ClientExcelExportService
    {
        public byte[] ExportToExcel(IEnumerable<ClientDto> clients)
        {
            var clientList = clients.ToList();

            using var package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("Clients");

            sheet.Cells["A1:G1"].Merge = true;
            sheet.Cells["A1"].Value = "Clients Report";
            sheet.Cells["A1"].Style.Font.Size = 16;
            sheet.Cells["A1"].Style.Font.Bold = true;
            sheet.Cells["A1"].Style.Font.Color.SetColor(Color.FromArgb(26, 115, 232));
            sheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Row(1).Height = 28;

            sheet.Cells["A2:G2"].Merge = true;
            sheet.Cells["A2"].Value =
                $"Generated on {DateTime.Now:dd/MM/yyyy HH:mm}  —  {clientList.Count} records";
            sheet.Cells["A2"].Style.Font.Size = 10;
            sheet.Cells["A2"].Style.Font.Color.SetColor(Color.Gray);
            sheet.Cells["A2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Row(2).Height = 18;

            string[] headers = { "#", "Client Name", "Phone", "Project",
                                  "Type", "Sales Agent", "Date" };
            int headerRow = 4;

            for (int c = 0; c < headers.Length; c++)
            {
                var cell = sheet.Cells[headerRow, c + 1];
                cell.Value = headers[c];
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 11;
                cell.Style.Font.Color.SetColor(Color.White);
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(26, 115, 232));
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.White);
            }
            sheet.Row(headerRow).Height = 22;

            int row = headerRow + 1;
            int i = 1;

            foreach (var c in clientList)
            {
                bool isEven = i % 2 == 0;
                var bgColor = isEven ? Color.FromArgb(245, 247, 250) : Color.White;

                sheet.Cells[row, 1].Value = i;
                sheet.Cells[row, 2].Value = c.ClientName;
                sheet.Cells[row, 3].Value = c.Phone;
                sheet.Cells[row, 4].Value = c.ProjectName;
                sheet.Cells[row, 5].Value = c.Type == "Walk" ? "Walk-in" : "Follow-up";
                sheet.Cells[row, 6].Value = c.AgentName;
                sheet.Cells[row, 7].Value = c.CreatedAt.ToString("dd/MM/yyyy");

                using (var range = sheet.Cells[row, 1, row, 7])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(bgColor);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(Color.FromArgb(230, 230, 230));
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.Font.Size = 10.5f;
                }

                sheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                var typeCell = sheet.Cells[row, 5];
                if (c.Type == "Walk")
                {
                    typeCell.Style.Font.Color.SetColor(Color.FromArgb(52, 199, 89));
                    typeCell.Style.Font.Bold = true;
                }
                else
                {
                    typeCell.Style.Font.Color.SetColor(Color.FromArgb(255, 149, 0));
                    typeCell.Style.Font.Bold = true;
                }

                row++;
                i++;
            }

            sheet.Column(1).Width = 6;
            sheet.Column(2).Width = 25;
            sheet.Column(3).Width = 18;
            sheet.Column(4).Width = 20;
            sheet.Column(5).Width = 14;
            sheet.Column(6).Width = 20;
            sheet.Column(7).Width = 14;

            sheet.View.FreezePanes(headerRow + 1, 1);

            return package.GetAsByteArray();
        }
    }
}