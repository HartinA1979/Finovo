namespace Finovo.Application.Common.Interfaces;

public interface IPdfService
{
    byte[] GenerateInvoicePdf(InvoicePdfData data);
}

public sealed record InvoicePdfData(
    string InvoiceNumber,
    DateTime IssueDate,
    DateTime DueDate,
    // Seller (Tenant)
    string SellerName,
    string? SellerCompany,
    string? SellerEmail,
    string? SellerPhone,
    string? SellerAddress,
    string? SellerVatNumber,
    // Buyer (Customer)
    string BuyerName,
    string? BuyerCompany,
    string? BuyerEmail,
    string? BuyerAddress,
    string? BuyerVatNumber,
    // Content
    string? Notes,
    IReadOnlyList<InvoicePdfItem> Items,
    decimal TotalNet,
    decimal TaxAmount,
    decimal TotalGross,
    string Currency);

public sealed record InvoicePdfItem(
    string Description,
    decimal Hours,
    decimal HourlyRate,
    int TaxRatePercent,
    decimal TotalNet,
    decimal TaxAmount,
    decimal TotalGross);
