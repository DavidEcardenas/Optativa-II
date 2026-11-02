using System.Net;
using System.Net.Http.Json;
using LastMile.Api.Dtos;
using LastMile.Domain.Entregas;
using LastMile.Infrastructure.Persistence;
using LastMile.Integration.Tests.Fixtures;
using Microsoft.Extensions.DependencyInjection;

namespace LastMile.Integration.Tests;

public class AssignCourierEndpointTests : IClassFixture<LastMileWebAppFactory>
{
    private readonly LastMileWebAppFactory _factory;

    public AssignCourierEndpointTests(LastMileWebAppFactory factory)
        => _factory = factory;

    // Camino feliz
    [Fact]
    public async Task Post_ValidRequest_ReturnsOkAndPersists()
    {
        var client = _factory.CreateClient();
        var delivery = await SeedPreparedDeliveryAsync();

        var response = await client.PostAsJsonAsync(
            $"/deliveries/{delivery.Id.Value}/assign-courier",
            new AssignCourierRequest(
                CourierId.New().Value,
                "admin@lastmile.com"));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<LastMileDbContext>();
        var saved = await db.Deliveries.FindAsync(delivery.Id);
        Assert.NotNull(saved);
        Assert.Equal(DeliveryStatus.Asignada, saved!.Status);
    }

    // Escenario de error: entrega inexistente
    [Fact]
    public async Task Post_UnknownDelivery_ReturnsNotFound()
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync(
            $"/deliveries/{Guid.NewGuid()}/assign-courier",
            new AssignCourierRequest(Guid.NewGuid(), "admin@lastmile.com"));

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // Escenario de error: RequestedBy vacío
    [Fact]
    public async Task Post_EmptyRequestedBy_ReturnsBadRequest()
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync(
            $"/deliveries/{Guid.NewGuid()}/assign-courier",
            new AssignCourierRequest(Guid.NewGuid(), ""));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    private async Task<Delivery> SeedPreparedDeliveryAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<LastMileDbContext>();

        var delivery = new Delivery(
            DeliveryId.New(),
            OrderId.New(),
            new DeliveryAddress("Calle 100 #15-20", "Bogotá", 4.68, -74.05),
            new DeliveryWindow(
                DateTime.UtcNow.AddHours(1),
                DateTime.UtcNow.AddHours(3)));
        delivery.MarkAsPrepared();

        db.Deliveries.Add(delivery);
        await db.SaveChangesAsync();
        return delivery;
    }
}