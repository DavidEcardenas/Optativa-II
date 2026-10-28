using LastMile.Api.Middleware;
using LastMile.Application.Entregas;
using LastMile.Application.Entregas.Ports;
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

builder.Services.AddHttpClient<ICourierAvailabilityPort,
    HttpCourierAvailabilityAdapter>(c =>
    c.BaseAddress = new Uri(builder.Configuration["ExternalApis:Couriers"]!));

builder.Services.AddHttpClient<IRouteCapacityPort,
    HttpRouteCapacityAdapter>(c =>
    c.BaseAddress = new Uri(builder.Configuration["ExternalApis:Couriers"]!));

builder.Services.AddHttpClient<IStockReservationPort,
    HttpStockReservationAdapter>(c =>
    c.BaseAddress = new Uri(builder.Configuration["ExternalApis:Orders"]!));

builder.Services.AddSingleton<IEventPublisher, InMemoryEventPublisher>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();