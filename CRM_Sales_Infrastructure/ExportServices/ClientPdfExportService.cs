using CRM_Sales_Application.DTOs;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace CRM_Sales_Infrastructure.ExportServices
{
    public class ClientPdfExportService
    {
        public byte[] ExportToPdf(IEnumerable<ClientDto> clients)
        {
            var clientList = clients.ToList();
            var accentColor = "#1A73E8";

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Size(PageSizes.A4.Landscape());
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                    page.Header().Column(col =>
                    {
                        col.Item().Text("Clients Report")
                            .FontSize(20).Bold().FontColor(accentColor);

                        col.Item().PaddingTop(2).Text(
                            $"Generated on {DateTime.Now:dd/MM/yyyy HH:mm}  —  {clientList.Count} records")
                            .FontSize(9).FontColor(Colors.Grey.Darken1);

                        col.Item().PaddingTop(8).LineHorizontal(1).LineColor(accentColor);
                    });

                    page.Content().PaddingTop(15).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(35);
                            columns.RelativeColumn(2.2f);
                            columns.RelativeColumn(1.6f);
                            columns.RelativeColumn(1.8f);
                            columns.RelativeColumn(1.3f);
                            columns.RelativeColumn(1.8f);
                            columns.RelativeColumn(1.3f);
                        });

                        table.Header(header =>
                        {
                            string[] headers = { "#", "Client Name", "Phone", "Project",
                                                  "Type", "Sales Agent", "Date" };
                            foreach (var h in headers)
                            {
                                header.Cell()
                                    .Background(accentColor)
                                    .Padding(8)
                                    .Text(h)
                                    .FontColor(Colors.White)
                                    .Bold()
                                    .FontSize(10);
                            }
                        });

                        int i = 1;
                        foreach (var c in clientList)
                        {
                            bool isEven = i % 2 == 0;
                            var bg = isEven ? Colors.Grey.Lighten4 : Colors.White;
                            var typeColor = c.Type == "Walk" ? "#34C759" : "#FF9500";
                            var typeText = c.Type == "Walk" ? "Walk-in" : "Follow-up";

                            table.Cell().Background(bg).Padding(7)
                                .AlignCenter().Text(i.ToString());
                            table.Cell().Background(bg).Padding(7).Text(c.ClientName);
                            table.Cell().Background(bg).Padding(7).Text(c.Phone);
                            table.Cell().Background(bg).Padding(7).Text(c.ProjectName);
                            table.Cell().Background(bg).Padding(7)
                                .Text(typeText).FontColor(typeColor).Bold();
                            table.Cell().Background(bg).Padding(7).Text(c.AgentName);
                            table.Cell().Background(bg).Padding(7)
                                .Text(c.CreatedAt.ToString("dd/MM/yyyy"));

                            i++;
                        }
                    });

                    page.Footer().PaddingTop(10).Row(row =>
                    {
                        row.RelativeItem().Text("CRM Sales — Sales Management System")
                            .FontSize(8).FontColor(Colors.Grey.Darken1);

                        row.RelativeItem().AlignRight().Text(x =>
                        {
                            x.Span("Page ").FontSize(8);
                            x.CurrentPageNumber().FontSize(8);
                            x.Span(" of ").FontSize(8);
                            x.TotalPages().FontSize(8);
                        });
                    });
                });
            });

            return document.GeneratePdf();
        }
    }
}