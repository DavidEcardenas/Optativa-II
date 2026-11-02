using LastMile.Api.Middleware;
using LastMile.Application.Entregas;
using LastMile.Application.Entregas.Ports;
using LastMile.Domain.Entregas;
using LastMile.Infrastructure.Events;
using LastMile.Infrastructure.External;
using LastMile.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<LastMileDbContext>(options =>
    options.UseNpgsql(builder.Configuration
        .GetConnectionString("LastMile")));

builder.Services.AddScoped<IDeliveryRepository, EfDeliveryRepository>();
builder.Services.AddScoped<AssignCourierUseCase>();
builder.Services.AddSingleton<IEventPublisher, InMemoryEventPublisher>();

// Adaptadores externos: si no hay backend configurado, usa dobles.
var couriersBaseUrl = builder.Configuration["ExternalApis:Couriers"];
var ordersBaseUrl = builder.Configuration["ExternalApis:Orders"];

if (string.IsNullOrWhiteSpace(couriersBaseUrl)
    || string.IsNullOrWhiteSpace(ordersBaseUrl))
{
    builder.Services.AddSingleton<ICourierAvailabilityPort,
        DemoCourierAvailabilityPort>();
    builder.Services.AddSingleton<IRouteCapacityPort,
        DemoRouteCapacityPort>();
    builder.Services.AddSingleton<IStockReservationPort,
        DemoStockReservationPort>();
}
else
{
    builder.Services.AddHttpClient<ICourierAvailabilityPort,
        HttpCourierAvailabilityAdapter>(c =>
        c.BaseAddress = new Uri(couriersBaseUrl));

    builder.Services.AddHttpClient<IRouteCapacityPort,
        HttpRouteCapacityAdapter>(c =>
        c.BaseAddress = new Uri(couriersBaseUrl));

    builder.Services.AddHttpClient<IStockReservationPort,
        HttpStockReservationAdapter>(c =>
        c.BaseAddress = new Uri(ordersBaseUrl));
}

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();

// Dobles para modo demo (sin servicios externos)
internal sealed class DemoCourierAvailabilityPort : ICourierAvailabilityPort
{
    public Task<bool> IsAvailableAsync(CourierId id, CancellationToken ct)
        => Task.FromResult(true);
}

internal sealed class DemoRouteCapacityPort : IRouteCapacityPort
{
    public Task<bool> HasCapacityAsync(CourierId id, CancellationToken ct)
        => Task.FromResult(true);
}

internal sealed class DemoStockReservationPort : IStockReservationPort
{
    public Task<bool> IsReservedAsync(OrderId id, CancellationToken ct)
        => Task.FromResult(true);
}

public partial class Program { }