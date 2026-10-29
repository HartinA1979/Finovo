using System.Net.Http.Json;
using Finovo.Client.Models;

namespace Finovo.Client.Services;

public sealed class InvoiceService
{
    private readonly HttpClient _http;

    public InvoiceService(HttpClient http) => _http = http;

    public async Task<List<InvoiceSummaryModel>> GetAllAsync(
        Guid? customerId = null,
        string? status = null,
        CancellationToken ct = default)
    {
        var query = new List<string>();
        if (customerId.HasValue)
            query.Add("customerId=" + customerId.Value);
        if (!string.IsNullOrWhiteSpace(status))
            query.Add("status=" + Uri.EscapeDataString(status));

        var url = "api/invoices" + (query.Count > 0 ? "?" + string.Join("&", query) : "");
        return await _http.GetFromJsonAsync<List<InvoiceSummaryModel>>(url, ct) ?? [];
    }

    public async Task<InvoiceModel?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _http.GetFromJsonAsync<InvoiceModel>($"api/invoices/{id}", ct);

    public async Task<Guid> CreateAsync(CreateInvoiceRequest request, CancellationToken ct = default)
    {
        var response = await _http.PostAsJsonAsync("api/invoices", request, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Guid>(ct);
    }

    public async Task UpdateDraftAsync(Guid id, UpdateInvoiceDraftRequest request, CancellationToken ct = default)
    {
        var response = await _http.PutAsJsonAsync($"api/invoices/{id}", request, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task SendAsync(Guid id, CancellationToken ct = default)
    {
        var response = await _http.PostAsync($"api/invoices/{id}/send", null, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task PayAsync(Guid id, CancellationToken ct = default)
    {
        var response = await _http.PostAsync($"api/invoices/{id}/pay", null, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task CancelAsync(Guid id, CancellationToken ct = default)
    {
        var response = await _http.PostAsync($"api/invoices/{id}/cancel", null, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task<byte[]> GetPdfAsync(Guid id, CancellationToken ct = default)
    {
        var response = await _http.GetAsync($"api/invoices/{id}/pdf", ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsByteArrayAsync(ct);
    }

    public async Task<Guid> AddItemAsync(Guid invoiceId, InvoiceItemRequest request, CancellationToken ct = default)
    {
        var response = await _http.PostAsJsonAsync($"api/invoices/{invoiceId}/items", request, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Guid>(ct);
    }

    public async Task UpdateItemAsync(Guid invoiceId, Guid itemId, InvoiceItemRequest request, CancellationToken ct = default)
    {
        var response = await _http.PutAsJsonAsync($"api/invoices/{invoiceId}/items/{itemId}", request, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task RemoveItemAsync(Guid invoiceId, Guid itemId, CancellationToken ct = default)
    {
        var response = await _http.DeleteAsync($"api/invoices/{invoiceId}/items/{itemId}", ct);
        response.EnsureSuccessStatusCode();
    }
}
