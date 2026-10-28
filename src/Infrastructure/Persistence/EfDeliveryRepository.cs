using LastMile.Application.Entregas.Ports;
using LastMile.Domain.Entregas;
using Microsoft.EntityFrameworkCore;

namespace LastMile.Infrastructure.Persistence;

public sealed class EfDeliveryRepository : IDeliveryRepository
{
    private readonly LastMileDbContext _db;

    public EfDeliveryRepository(LastMileDbContext db) => _db = db;

    public Task<Delivery?> GetAsync(DeliveryId id, CancellationToken ct)
        => _db.Deliveries.FirstOrDefaultAsync(d => d.Id == id, ct);

    public async Task SaveAsync(Delivery delivery, CancellationToken ct)
    {
        _db.Deliveries.Update(delivery);
        await _db.SaveChangesAsync(ct);
    }
}