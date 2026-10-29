using Finovo.Application.Common.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Finovo.Infrastructure.Services;

public sealed class InvoicePdfService : IPdfService
{
    static InvoicePdfService()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public byte[] GenerateInvoicePdf(InvoicePdfData data)
    {
        var doc = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                page.Header().Element(c => ComposeHeader(c, data));
                page.Content().Element(c => ComposeContent(c, data));
                page.Footer().AlignCenter().Text(t =>
                {
                    t.Span("Seite ");
                    t.CurrentPageNumber();
                    t.Span(" von ");
                    t.TotalPages();
                });
            });
        });

        return doc.GeneratePdf();
    }

    private static void ComposeHeader(IContainer container, InvoicePdfData data)
    {
        container.Column(col =>
        {
            col.Item().Row(row =>
            {
                // Rechnungs-Titel
                row.RelativeItem().Column(c =>
                {
                    c.Item().Text("RECHNUNG").FontSize(22).Bold().FontColor(Colors.Grey.Darken3);
                    c.Item().Text(data.InvoiceNumber).FontSize(14).FontColor(Colors.Grey.Darken1);
                });

                // Datum-Info
                row.ConstantItem(180).Column(c =>
                {
                    c.Item().Row(r =>
                    {
                        r.RelativeItem().Text("Ausstellungsdatum:").SemiBold();
                        r.ConstantItem(100).AlignRight().Text(data.IssueDate.ToString("dd.MM.yyyy"));
                    });
                    c.Item().Row(r =>
                    {
                        r.RelativeItem().Text("Fälligkeitsdatum:").SemiBold();
                        r.ConstantItem(100).AlignRight().Text(data.DueDate.ToString("dd.MM.yyyy"));
                    });
                });
            });

            col.Item().PaddingTop(16).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
        });
    }

    private static void ComposeContent(IContainer container, InvoicePdfData data)
    {
        container.Column(col =>
        {
            col.Spacing(16);

            // Rechnungssteller | Rechnungsempfänger
            col.Item().Row(row =>
            {
                row.RelativeItem().Column(c =>
                {
                    c.Item().Text("Von").FontSize(9).FontColor(Colors.Grey.Medium).Bold();
                    c.Item().Text(data.SellerCompany ?? data.SellerName).SemiBold();
                    if (data.SellerCompany != null)
                        c.Item().Text(data.SellerName);
                    if (data.SellerAddress != null)
                        c.Item().Text(data.SellerAddress);
                    if (data.SellerEmail != null)
                        c.Item().Text(data.SellerEmail);
                    if (data.SellerPhone != null)
                        c.Item().Text(data.SellerPhone);
                    if (data.SellerVatNumber != null)
                        c.Item().Text($"UID: {data.SellerVatNumber}");
                });

                row.ConstantItem(20);

                row.RelativeItem().Column(c =>
                {
                    c.Item().Text("An").FontSize(9).FontColor(Colors.Grey.Medium).Bold();
                    c.Item().Text(data.BuyerCompany ?? data.BuyerName).SemiBold();
                    if (data.BuyerCompany != null)
                        c.Item().Text(data.BuyerName);
                    if (data.BuyerAddress != null)
                        c.Item().Text(data.BuyerAddress);
                    if (data.BuyerEmail != null)
                        c.Item().Text(data.BuyerEmail);
                    if (data.BuyerVatNumber != null)
                        c.Item().Text($"UID: {data.BuyerVatNumber}");
                });
            });

            // Positionen
            col.Item().Table(table =>
            {
                table.ColumnsDefinition(cols =>
                {
                    cols.RelativeColumn(5);   // Beschreibung
                    cols.RelativeColumn(1.2f); // Stunden
                    cols.RelativeColumn(1.5f); // Satz
                    cols.RelativeColumn(1f);   // MwSt
                    cols.RelativeColumn(1.5f); // Netto
                    cols.RelativeColumn(1.5f); // Brutto
                });

                // Header
                table.Header(header =>
                {
                    static IContainer HeaderCell(IContainer c) =>
                        c.DefaultTextStyle(t => t.SemiBold().FontColor(Colors.White))
                         .Background(Colors.Grey.Darken2)
                         .PaddingVertical(5)
                         .PaddingHorizontal(6);

                    header.Cell().Element(HeaderCell).Text("Beschreibung");
                    header.Cell().Element(HeaderCell).AlignRight().Text("Stunden");
                    header.Cell().Element(HeaderCell).AlignRight().Text($"Satz ({data.Currency})");
                    header.Cell().Element(HeaderCell).AlignRight().Text("MwSt.");
                    header.Cell().Element(HeaderCell).AlignRight().Text($"Netto ({data.Currency})");
                    header.Cell().Element(HeaderCell).AlignRight().Text($"Brutto ({data.Currency})");
                });

                // Zeilen
                var rowBg = Colors.Grey.Lighten5;
                for (var i = 0; i < data.Items.Count; i++)
                {
                    var item = data.Items[i];
                    var bg = i % 2 == 0 ? Colors.White : rowBg;

                    static IContainer DataCell(IContainer c, string bg) =>
                        c.Background(bg).PaddingVertical(5).PaddingHorizontal(6);

                    table.Cell().Element(c => DataCell(c, bg)).Text(item.Description);
                    table.Cell().Element(c => DataCell(c, bg)).AlignRight().Text(item.Hours.ToString("F2"));
                    table.Cell().Element(c => DataCell(c, bg)).AlignRight().Text(item.HourlyRate.ToString("F2"));
                    table.Cell().Element(c => DataCell(c, bg)).AlignRight().Text($"{item.TaxRatePercent} %");
                    table.Cell().Element(c => DataCell(c, bg)).AlignRight().Text(item.TotalNet.ToString("F2"));
                    table.Cell().Element(c => DataCell(c, bg)).AlignRight().Text(item.TotalGross.ToString("F2"));
                }
            });

            // Summen-Block
            col.Item().AlignRight().Width(220).Table(table =>
            {
                table.ColumnsDefinition(cols =>
                {
                    cols.RelativeColumn();
                    cols.ConstantColumn(100);
                });

                static void NormalRow(TableDescriptor t, string label, string value)
                {
                    t.Cell().PaddingVertical(3).PaddingHorizontal(6).Text(label);
                    t.Cell().AlignRight().PaddingVertical(3).PaddingHorizontal(6).Text(value);
                }

                static void BoldRow(TableDescriptor t, string label, string value)
                {
                    t.Cell().PaddingVertical(3).PaddingHorizontal(6).Text(label).SemiBold();
                    t.Cell().AlignRight().PaddingVertical(3).PaddingHorizontal(6).Text(value).SemiBold();
                }

                NormalRow(table, "Nettobetrag:", $"{data.TotalNet:F2} {data.Currency}");
                NormalRow(table, "MwSt.:", $"{data.TaxAmount:F2} {data.Currency}");

                table.Cell().ColumnSpan(2).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);

                BoldRow(table, "Gesamtbetrag:", $"{data.TotalGross:F2} {data.Currency}");
            });

            // Notizen
            if (!string.IsNullOrWhiteSpace(data.Notes))
            {
                col.Item().Column(c =>
                {
                    c.Item().Text("Anmerkungen").FontSize(9).FontColor(Colors.Grey.Medium).Bold();
                    c.Item().PaddingTop(4).Text(data.Notes).Italic();
                });
            }
        });
    }
}
