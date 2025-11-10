
using Org.BouncyCastle.Asn1.Ocsp;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using WaslaApp.Data.Models.admin.reports;

namespace Wasla_App.Models
{
    public class SummaryInvoiceReportPdf
    {
        public static byte[] GenerateAsync(string date_from, string date_to, List<SummaryInvoiceResponse> invoices)
        {
            string brandPurple = "#542d72";
            string brandGreen = "#00bc82";

            // decimal totalSalesAmount = 0, totalSalesTax = 0, totalPurchaseAmount = 0, totalPurchaseTax = 0;try{
            try
            {
                // Download logo from URL
                //string LogoUrl = "https://api.waslaa.de/images/logo.png";
                //byte[] logoBytes = null;
                //if (!string.IsNullOrEmpty(LogoUrl))
                //{
                //    using var httpClient = new HttpClient();
                //    logoBytes = await httpClient.GetByteArrayAsync(LogoUrl);
                //}
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
                                    .FontSize(16).Bold().FontColor(brandGreen);

                                // Table for currency
                                mainCol.Item().Table(table =>
                                {
                                    table.ColumnsDefinition(c =>
                                    {
                                        c.RelativeColumn(2); // Invoice No
                                        c.RelativeColumn(2); // Date
                                        c.RelativeColumn(3); // Customer
                                        c.RelativeColumn(2); // status
                                        c.RelativeColumn(2); // net
                                        c.RelativeColumn(1); // discount
                                        c.RelativeColumn(1); // Tax
                                        c.RelativeColumn(2); // total

                                    });

                                    // Header Row
                                    table.Header(header =>
                                    {
                                        //header.Cell().Element(c => HeaderCell(c, brandPurple)).Text("#").Bold();
                                        header.Cell().Element(c => PDFStyle.HeaderCell(c, brandPurple)).AlignCenter().Text("Invoice Number").Bold();
                                        header.Cell().Element(c => PDFStyle.HeaderCell(c, brandPurple)).AlignCenter().Text("Date").Bold();
                                        header.Cell().Element(c => PDFStyle.HeaderCell(c, brandPurple)).AlignCenter().Text("Customer").Bold();
                                        header.Cell().Element(c => PDFStyle.HeaderCell(c, brandPurple)).AlignCenter().Text("Status").Bold();
                                        header.Cell().Element(c => PDFStyle.HeaderCell(c, brandPurple)).AlignCenter().Text("Net Value").Bold();
                                        header.Cell().Element(c => PDFStyle.HeaderCell(c, brandPurple)).AlignCenter().Text("Discount").Bold();
                                        header.Cell().Element(c => PDFStyle.HeaderCell(c, brandPurple)).AlignCenter().Text("VAT").Bold();
                                        header.Cell().Element(c => PDFStyle.HeaderCell(c, brandPurple)).AlignCenter().Text("Total Amount").Bold();
                                    });

                                    int index = 1;
                                    foreach (var inv in group.invoices)
                                    {
                                        decimal? vatAmount = inv.total_price * inv.tax_amount;
                                        //table.Cell().Element(CellStyle).Text(index++);
                                        table.Cell().Element(PDFStyle.CellStyle).AlignCenter().Text(inv.invoice_code);
                                        table.Cell().Element(PDFStyle.CellStyle).AlignCenter().Text(inv.invoice_date?.ToString("yyyy-MM-dd"));
                                        table.Cell().Element(PDFStyle.CellStyle).AlignCenter().Text(inv.client_email);
                                        table.Cell().Element(PDFStyle.CellStyle).AlignCenter().Text(inv.status == 3 ? "Paid": "Not Paid");
                                        table.Cell().Element(PDFStyle.CellStyle).AlignCenter().Text(inv.total_price?.ToString("N2"));
                                        table.Cell().Element(PDFStyle.CellStyle).AlignCenter().Text(inv.copoun_discount_value?.ToString("N2"));
                                        table.Cell().Element(PDFStyle.CellStyle).AlignCenter().Text(vatAmount?.ToString("N2"));
                                        table.Cell().Element(PDFStyle.CellStyle).AlignCenter().Text(inv.grand_total_price?.ToString("N2"));
                                    }

                                    // Subtotal Row
                                    //decimal subtotalAmount = group?.NetValTotal;
                                    //decimal subtotalTax = group.Sum(g => g.Tax);
                                    //decimal subtotalTotal = group.Sum(g => g.Total);

                                    table.Cell().ColumnSpan(4).Element(PDFStyle.CellStyle).AlignCenter().Text("Grand total").Bold();
                                    table.Cell().Element(PDFStyle.CellStyle).AlignCenter().Text(group.NetValTotal?.ToString("N2")).Bold();
                                    table.Cell().Element(PDFStyle.CellStyle).AlignCenter().Text(group.GrandTotalDiscount?.ToString("N2")).Bold();
                                    table.Cell().Element(PDFStyle.CellStyle).AlignCenter().Text(group.GrandTotalVat?.ToString("N2")).Bold();
                                    table.Cell().Element(PDFStyle.CellStyle).AlignCenter().Text(group.GrandTotalAmount?.ToString("N2")).Bold();
                                });
                            }

                            // Grand Total for all currencies
                            mainCol.Item().PaddingVertical(10).Text("Grand Totals by Currency")
                                .FontSize(14).Bold().FontColor(brandPurple);

                            foreach (var group in invoices)
                            {
                                // Table for currency
                                mainCol.Item().AlignLeft().Shrink().Table(table =>
                                {
                                    table.ColumnsDefinition(c =>
                                    {
                                        c.ConstantColumn(70);  // currency ~ fits "USD", "EUR", etc.
                                        c.ConstantColumn(110);  // value


                                    });
                                    decimal? subtotalTotal = group.invoices?.Sum(g => g.grand_total_price);
                                    table.Cell().Element(PDFStyle.MiniCellStyle).AlignCenter().Text(group.currency_code).Bold();
                                    table.Cell().Element(PDFStyle.MiniCellStyle).AlignCenter().Text(subtotalTotal?.ToString("N2"));


                                });

                                   
                                //decimal? subtotalTotal = group.invoices?.Sum(g => g.grand_total_price);
                                //mainCol.Item().PaddingVertical(5).Text($"{group.currency_code}: {subtotalTotal:N2}").Bold().FontColor(brandGreen);
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
            catch(Exception ex)
            {
                return null;
            }
            
          
        }

      
    }
}
