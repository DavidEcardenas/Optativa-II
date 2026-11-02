namespace LastMile.Domain.Entregas;

public sealed class Delivery
{
    public DeliveryId Id { get; private set; } = default!;
    public OrderId OrderId { get; private set; } = default!;
    public DeliveryAddress Address { get; private set; } = default!;
    public DeliveryWindow Window { get; private set; } = default!;
    public DeliveryStatus Status { get; private set; }
    public CourierId? CourierId { get; private set; }
    public DateTime? AssignedAt { get; private set; }

    // Constructor vacío para EF Core
    private Delivery() { }

    public Delivery(
        DeliveryId id,
        OrderId orderId,
        DeliveryAddress address,
        DeliveryWindow window)
    {
        Id = id ?? throw new DomainException("DeliveryId is required");
        OrderId = orderId ?? throw new DomainException("OrderId is required");
        Address = address ?? throw new DomainException("Address is required");
        Window = window ?? throw new DomainException("Window is required");
        Status = DeliveryStatus.Pendiente;
    }

    public void MarkAsPrepared()
    {
        if (Status != DeliveryStatus.Pendiente)
            throw new DomainException(
                $"Cannot prepare from state {Status}");

        Status = DeliveryStatus.Preparada;
    }

    public void AssignCourier(CourierId courierId, string assignedBy)
    {
        // INV-06: no reasignar entregas cerradas (PRIMERO)
        if (Status is DeliveryStatus.Entregada or DeliveryStatus.Fallida)
            throw new DomainException("Cannot assign a closed delivery");

        // INV-01: no doble asignación
        if (CourierId is not null)
            throw new DomainException("Delivery already assigned");

        // Transición válida: solo desde Preparada
        if (Status != DeliveryStatus.Preparada)
            throw new DomainException(
                $"Cannot assign from state {Status}");

        if (string.IsNullOrWhiteSpace(assignedBy))
            throw new DomainException("AssignedBy is required");

        CourierId = courierId
            ?? throw new DomainException("CourierId is required");
        AssignedAt = DateTime.UtcNow;
        Status = DeliveryStatus.Asignada;
    }

    public void StartRoute()
    {
        if (Status != DeliveryStatus.Asignada)
            throw new DomainException(
                $"Cannot start route from state {Status}");

        Status = DeliveryStatus.EnRuta;
    }

    public void ConfirmDelivery()
    {
        if (Status != DeliveryStatus.EnRuta)
            throw new DomainException(
                $"Cannot confirm from state {Status}");

        Status = DeliveryStatus.Entregada;
    }

    public void ReportIncident()
    {
        if (Status != DeliveryStatus.EnRuta)
            throw new DomainException(
                $"Cannot report incident from state {Status}");

        Status = DeliveryStatus.ConIncidencia;
    }

    public void ResolveIncident()
    {
        if (Status != DeliveryStatus.ConIncidencia)
            throw new DomainException(
                $"Cannot resolve incident from state {Status}");

        Status = DeliveryStatus.EnRuta;
    }

    public void MarkAsFailed()
    {
        if (Status != DeliveryStatus.ConIncidencia)
            throw new DomainException(
                $"Cannot fail from state {Status}");

        Status = DeliveryStatus.Fallida;
    }
}