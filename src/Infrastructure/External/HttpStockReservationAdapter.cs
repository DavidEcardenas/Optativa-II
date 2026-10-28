using System.Net.Http.Json;
using LastMile.Application.Entregas.Ports;
using LastMile.Domain.Entregas;

namespace LastMile.Infrastructure.External;

public sealed class HttpStockReservationAdapter : IStockReservationPort
{
    private readonly HttpClient _http;

    public HttpStockReservationAdapter(HttpClient http) => _http = http;

    public async Task<bool> IsReservedAsync(OrderId orderId, CancellationToken ct)
    {
        var response = await _http.GetAsync(
            $"orders/{orderId.Value}/stock-reservation", ct);

        if (!response.IsSuccessStatusCode) return false;

        var payload = await response.Content
            .ReadFromJsonAsync<ReservationResponse>(cancellationToken: ct);

        return payload?.Reserved ?? false;
    }

    private sealed record ReservationResponse(bool Reserved);
}