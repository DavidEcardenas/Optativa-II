using LastMile.Application.Entregas;
using Microsoft.Extensions.Logging;

namespace LastMile.Infrastructure.Events;

public sealed class DeliveryAssignedHandler
{
    private readonly ILogger<DeliveryAssignedHandler> _logger;

    public DeliveryAssignedHandler(ILogger<DeliveryAssignedHandler> logger)
        => _logger = logger;

    public Task HandleAsync(DeliveryAssigned evt, CancellationToken ct)
    {
        _logger.LogInformation(
            "Delivery {DeliveryId} assigned to courier {CourierId} at {OccurredAt}",
            evt.DeliveryId, evt.CourierId, evt.OccurredAt);
        return Task.CompletedTask;
    }
}