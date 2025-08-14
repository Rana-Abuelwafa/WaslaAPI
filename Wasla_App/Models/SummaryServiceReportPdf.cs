using WaslaApp.Data.Models.admin.reports;
using Org.BouncyCastle.Asn1.Ocsp;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
namespace Wasla_App.Models
{
    public class SummaryServiceReportPdf
    {
        public static byte[] GenerateAsync(string date_from, string date_to, List<SummaryServiceResponseCurr> invoices)
        {
            string brandPurple = "#542d72";
            string brandGreen = "#00bc82";

            // decimal totalSalesAmount = 0, totalSalesTax = 0, totalPurchaseAmount = 0, totalPurchaseTax = 0;try{
            try
            {
                var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "logo.png");
                return Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.MarginHorizontal(20);
                        page.MarginVertical(30);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(8).FontColor(brandPurple));

                        // HEADER
                        page.Header().Row(row =>
                        {

                            row.RelativeItem().Column(col =>
                            {
                                col.Item().PaddingVertical(10).Text("Invoice Summary Report")
                                    .FontSize(14).Bold().FontColor(brandPurple);
                                col.Item().PaddingVertical(2).Text($"Date From: {date_from}");
                                col.Item().PaddingVertical(2).Text($"Date To: {date_to}");
                                col.Item().PaddingVertical(2).Text($"Generated on: {DateTime.Now:yyyy-MM-dd}");
                            });

                            row.RelativeItem().AlignRight().Height(25).Image(logoPath);


                        });

                        // CONTENT
                        page.Content().PaddingVertical(10).Column(mainCol =>
                        {
                            foreach (var group in invoices)
                            {
                                string currency = group.currency_code;

                                // Currency Header
                                mainCol.Item().PaddingVertical(10).Text($"{currency} Currency")
                                    .FontSize(14).Bold().FontColor("#222");

                                // Table for currency
                                mainCol.Item().Table(table =>
                                {
                                    table.ColumnsDefinition(c =>
                                    {
                                        c.RelativeColumn(2); // service
                                        c.RelativeColumn(2); // net
                                        c.RelativeColumn(1); // discount
                                        c.RelativeColumn(1); // Tax
                                        c.RelativeColumn(2); // total

                                    });
                                    // Header Row
                                    table.Header(header =>
                                    {
                                        //header.Cell().Element(c => HeaderCell(c, brandPurple)).Text("#").Bold();
                                        header.Cell().Element(c => PDFStyle.HeaderCell(c, brandPurple)).AlignCenter().Text("Service").Bold();
                                        header.Cell().Element(c => PDFStyle.HeaderCell(c, brandPurple)).AlignCenter().Text("Net Value").Bold();
                                        header.Cell().Element(c => PDFStyle.HeaderCell(c, brandPurple)).AlignCenter().Text("Discount").Bold();
                                        header.Cell().Element(c => PDFStyle.HeaderCell(c, brandPurple)).AlignCenter().Text("VAT").Bold();
                                        header.Cell().Element(c => PDFStyle.HeaderCell(c, brandPurple)).AlignCenter().Text("Total Amount").Bold();
                                    });

                                    int index = 1;
                                    foreach (var inv in group.result)
                                    {
                                        //table.Cell().Element(CellStyle).Text(index++);
                                        table.Cell().Element(PDFStyle.CellStyle).AlignCenter().Text(inv.service_name);
                                        table.Cell().Element(PDFStyle.CellStyle).AlignCenter().Text(inv.NetValTotal?.ToString("N2"));
                                        table.Cell().Element(PDFStyle.CellStyle).AlignCenter().Text(inv.GrandTotalDiscount?.ToString("N2"));
                                        table.Cell().Element(PDFStyle.CellStyle).AlignCenter().Text(inv.GrandTotalVat?.ToString("N2"));
                                        table.Cell().Element(PDFStyle.CellStyle).AlignCenter().Text(inv.GrandTotalAmount?.ToString("N2"));
                                    }
                                   
                                });
                            }

                            // Grand Total for all currencies
                            mainCol.Item().PaddingVertical(10).Text("Grand Totals by Currency")
                                .FontSize(14).Bold().FontColor(brandPurple);

                            foreach (var group in invoices)
                            {
                                decimal? subtotalTotal = group.result?.Sum(g => g.GrandTotalAmount);
                                mainCol.Item().PaddingVertical(5).Text($"{group.currency_code}: {subtotalTotal:N2}").Bold().FontColor(brandGreen);
                            }
                        });

                        // FOOTER
                        page.Footer().AlignCenter().Text(txt =>
                        {
                            txt.Span("Report generated by Wasla System — Page ");
                            txt.CurrentPageNumber();
                            txt.Span(" of ");
                            txt.TotalPages();
                        });
                    });
                }).GeneratePdf();
            }
            catch (Exception ex)
            {
                return null;
            }


        }
    }
}
