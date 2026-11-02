# Plan de implementación — Actividad 4

## Vertical slice

POST /deliveries/{id}/assign-courier
→ AssignCourierUseCase
→ Delivery (dominio)
→ EfDeliveryRepository (PostgreSQL)
→ InMemoryEventPublisher (evento DeliveryAssigned)

## Capas conectadas

| Capa | Pieza | Archivo |
|---|---|---|
| Entrada | Controller + DTO | DeliveriesController, AssignCourierRequest |
| Aplicación | Caso de uso | AssignCourierUseCase (Actividad 3) |
| Dominio | Entidad | Delivery (Actividad 3) |
| Infraestructura | Repositorio | EfDeliveryRepository |
| Infraestructura | Adaptadores | HttpCourierAvailabilityAdapter, HttpRouteCapacityAdapter, HttpStockReservationAdapter |
| Infraestructura | Eventos | InMemoryEventPublisher, DeliveryAssignedHandler |

## Decisiones de implementación

- Se usa EF Core con PostgreSQL (consistente con ADR-001).
- Los adaptadores HTTP usan `IHttpClientFactory`; en tests se reemplazan por dobles.
- El evento `DeliveryAssigned` se publica en un publicador en memoria; en producción
  se reemplazaría por RabbitMQ sin tocar el caso de uso.
- Las migraciones se generan con `dotnet ef migrations`.