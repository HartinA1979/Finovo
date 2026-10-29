using System.Net.Http.Json;
using Finovo.Client.Models;

namespace Finovo.Client.Services;

public sealed class ProjectService
{
    private readonly HttpClient _http;

    public ProjectService(HttpClient http) => _http = http;

    public async Task<List<ProjectSummaryModel>> GetAllAsync(
        Guid? customerId = null,
        string? status = null,
        CancellationToken ct = default)
    {
        var query = new List<string>();
        if (customerId.HasValue)
            query.Add("customerId=" + customerId.Value);
        if (!string.IsNullOrWhiteSpace(status))
            query.Add("status=" + Uri.EscapeDataString(status));

        var url = "api/projects" + (query.Count > 0 ? "?" + string.Join("&", query) : "");
        return await _http.GetFromJsonAsync<List<ProjectSummaryModel>>(url, ct) ?? [];
    }

    public async Task<ProjectModel?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _http.GetFromJsonAsync<ProjectModel>($"api/projects/{id}", ct);

    public async Task<Guid> CreateAsync(CreateProjectRequest request, CancellationToken ct = default)
    {
        var response = await _http.PostAsJsonAsync("api/projects", request, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Guid>(ct);
    }

    public async Task UpdateAsync(Guid id, UpdateProjectRequest request, CancellationToken ct = default)
    {
        var response = await _http.PutAsJsonAsync($"api/projects/{id}", request, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task CompleteAsync(Guid id, CancellationToken ct = default)
    {
        var response = await _http.PostAsync($"api/projects/{id}/complete", null, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task ReopenAsync(Guid id, CancellationToken ct = default)
    {
        var response = await _http.PostAsync($"api/projects/{id}/reopen", null, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task ArchiveAsync(Guid id, CancellationToken ct = default)
    {
        var response = await _http.DeleteAsync($"api/projects/{id}", ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task<Guid> AddServiceItemAsync(Guid projectId, ServiceItemRequest request, CancellationToken ct = default)
    {
        var response = await _http.PostAsJsonAsync($"api/projects/{projectId}/service-items", request, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Guid>(ct);
    }

    public async Task UpdateServiceItemAsync(Guid projectId, Guid itemId, ServiceItemRequest request, CancellationToken ct = default)
    {
        var response = await _http.PutAsJsonAsync($"api/projects/{projectId}/service-items/{itemId}", request, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task RemoveServiceItemAsync(Guid projectId, Guid itemId, CancellationToken ct = default)
    {
        var response = await _http.DeleteAsync($"api/projects/{projectId}/service-items/{itemId}", ct);
        response.EnsureSuccessStatusCode();
    }
}
