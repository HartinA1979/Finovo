using System.Net.Http.Json;
using Finovo.Client.Models;

namespace Finovo.Client.Services;

public sealed class CustomerService
{
    private readonly HttpClient _http;

    public CustomerService(HttpClient http) => _http = http;

    public async Task<List<CustomerModel>> GetAllAsync(string? nameFilter = null, bool? isActive = null, CancellationToken ct = default)
    {
        var query = new List<string>();
        if (!string.IsNullOrWhiteSpace(nameFilter))
            query.Add($"nameFilter={Uri.EscapeDataString(nameFilter)}");
        if (isActive.HasValue)
            query.Add($"isActive={isActive.Value.ToString().ToLower()}");

        var url = "api/customers" + (query.Count > 0 ? "?" + string.Join("&", query) : "");
        return await _http.GetFromJsonAsync<List<CustomerModel>>(url, ct) ?? [];
    }

    public async Task<CustomerModel?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _http.GetFromJsonAsync<CustomerModel>($"api/customers/{id}", ct);

    public async Task<Guid> CreateAsync(CreateCustomerRequest request, CancellationToken ct = default)
    {
        var response = await _http.PostAsJsonAsync("api/customers", request, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Guid>(ct);
    }

    public async Task UpdateAsync(Guid id, UpdateCustomerRequest request, CancellationToken ct = default)
    {
        var response = await _http.PutAsJsonAsync($"api/customers/{id}", request, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeactivateAsync(Guid id, CancellationToken ct = default)
    {
        var response = await _http.DeleteAsync($"api/customers/{id}", ct);
        response.EnsureSuccessStatusCode();
    }
}
