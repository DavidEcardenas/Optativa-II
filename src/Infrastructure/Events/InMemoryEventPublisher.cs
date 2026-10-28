using System.Collections.Concurrent;
using LastMile.Application.Entregas.Ports;

namespace LastMile.Infrastructure.Events;

public sealed class InMemoryEventPublisher : IEventPublisher
{
    private readonly ConcurrentQueue<object> _events = new();

    public IReadOnlyCollection<object> Published => _events.ToArray();

    public Task PublishAsync<TEvent>(TEvent @event, CancellationToken ct)
        where TEvent : class
    {
        _events.Enqueue(@event);
        return Task.CompletedTask;
    }
}