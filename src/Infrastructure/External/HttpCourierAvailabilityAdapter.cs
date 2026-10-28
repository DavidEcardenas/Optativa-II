using System.Net.Http.Json;
using LastMile.Application.Entregas.Ports;
using LastMile.Domain.Entregas;

namespace LastMile.Infrastructure.External;

public sealed class HttpCourierAvailabilityAdapter : ICourierAvailabilityPort
{
    private readonly HttpClient _http;

    public HttpCourierAvailabilityAdapter(HttpClient http) => _http = http;

    public async Task<bool> IsAvailableAsync(CourierId id, CancellationToken ct)
    {
        var response = await _http.GetAsync(
            $"couriers/{id.Value}/availability", ct);

        if (!response.IsSuccessStatusCode) return false;

        var payload = await response.Content
            .ReadFromJsonAsync<AvailabilityResponse>(cancellationToken: ct);

        return payload?.Available ?? false;
    }

    private sealed record AvailabilityResponse(bool Available);
}