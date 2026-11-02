using LastMile.Application.Entregas.Ports;
using LastMile.Domain.Entregas;
using LastMile.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LastMile.Integration.Tests.Fixtures;

public sealed class LastMileWebAppFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Reemplazar el DbContext por uno en memoria
            var descriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(DbContextOptions<LastMileDbContext>));
            if (descriptor is not null) services.Remove(descriptor);

            services.AddDbContext<LastMileDbContext>(o =>
                o.UseInMemoryDatabase("LastMileTests"));

            // Reemplazar puertos externos por dobles
            services.RemoveAll<ICourierAvailabilityPort>();
            services.AddSingleton<ICourierAvailabilityPort,
                AlwaysAvailableCourierPort>();

            services.RemoveAll<IRouteCapacityPort>();
            services.AddSingleton<IRouteCapacityPort,
                AlwaysHasCapacityRoutePort>();

            services.RemoveAll<IStockReservationPort>();
            services.AddSingleton<IStockReservationPort,
                AlwaysReservedStockPort>();
        });
    }

    private sealed class AlwaysAvailableCourierPort
        : ICourierAvailabilityPort
    {
        public Task<bool> IsAvailableAsync(CourierId id, CancellationToken ct)
            => Task.FromResult(true);
    }

    private sealed class AlwaysHasCapacityRoutePort : IRouteCapacityPort
    {
        public Task<bool> HasCapacityAsync(CourierId id, CancellationToken ct)
            => Task.FromResult(true);
    }

    private sealed class AlwaysReservedStockPort : IStockReservationPort
    {
        public Task<bool> IsReservedAsync(OrderId id, CancellationToken ct)
            => Task.FromResult(true);
    }
}