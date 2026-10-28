using System.Net.Http.Json;
using LastMile.Application.Entregas.Ports;
using LastMile.Domain.Entregas;

namespace LastMile.Infrastructure.External;

public sealed class HttpRouteCapacityAdapter : IRouteCapacityPort
{
    private readonly HttpClient _http;

    public HttpRouteCapacityAdapter(HttpClient http) => _http = http;

    public async Task<bool> HasCapacityAsync(CourierId id, CancellationToken ct)
    {
        var response = await _http.GetAsync(
            $"couriers/{id.Value}/capacity", ct);

        if (!response.IsSuccessStatusCode) return false;

        var payload = await response.Content
            .ReadFromJsonAsync<CapacityResponse>(cancellationToken: ct);

        return payload?.HasCapacity ?? false;
    }

    private sealed record CapacityResponse(bool HasCapacity);
}