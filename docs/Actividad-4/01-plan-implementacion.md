# Plan de implementación — Actividad 4

## Vertical slice implementado

POST /deliveries/{id}/assign-courier → DeliveriesController (entrada) → AssignCourierUseCase (aplicación) → Delivery (dominio) → EfDeliveryRepository (persistencia) → InMemoryEventPublisher (evento DeliveryAssigned)

## Capas conectadas

| Capa | Pieza | Archivo |
|---|---|---|
| Entrada | Controller + DTO | `src/Api/Controllers/DeliveriesController.cs`, `src/Api/Dtos/AssignCourierRequest.cs` |
| Entrada | Middleware de errores | `src/Api/Middleware/ExceptionHandlingMiddleware.cs` |
| Aplicación | Caso de uso | `src/Application/Entregas/AssignCourierUseCase.cs` |
| Aplicación | Contratos | `AssignCourierCommand.cs`, `AssignCourierResult.cs`, `DeliveryAssigned.cs` |
| Dominio | Entidad | `src/Domain/Entregas/Delivery.cs` |
| Dominio | Objetos de valor | `DeliveryAddress.cs`, `DeliveryWindow.cs`, `DeliveryStatus.cs` |
| Infraestructura | Persistencia | `EfDeliveryRepository.cs`, `LastMileDbContext.cs`, `DeliveryConfiguration.cs` |
| Infraestructura | Adaptadores | `HttpCourierAvailabilityAdapter.cs`, `HttpRouteCapacityAdapter.cs`, `HttpStockReservationAdapter.cs` |
| Infraestructura | Eventos | `InMemoryEventPublisher.cs`, `DeliveryAssignedHandler.cs` |

## Decisiones de implementación

1. **Persistencia con EF Core + PostgreSQL**, consistente con el ADR-001 (monolito modular).
2. **Migraciones reproducibles** generadas con `dotnet ef migrations add` y versionadas en el repo.
3. **Adaptadores HTTP con modo demo**: si `ExternalApis` no está configurado en `appsettings.json`, la API usa dobles que devuelven `true`. Los adaptadores HTTP reales siguen en el código y se activan al configurar las URLs.
4. **Publicador de eventos en memoria**: demuestra el flujo sin dependencia de RabbitMQ. Se anota como riesgo residual RR-01.
5. **Tests de integración con `WebApplicationFactory`** y base de datos en memoria, con dobles de los puertos externos.
6. **Fitness function de arquitectura** con NetArchTest para verificar que el dominio no dependa de infraestructura.

## Flujo end-to-end probado

| Caso | Método | Resultado |
|---|---|---|
| Asignación exitosa | POST válido | 200 + JSON |
| Entrega inexistente | POST con ID desconocido | 404 |
| RequestedBy vacío | POST sin `requestedBy` | 400 |
| Doble asignación | POST repetido | 409 |

## Evidencia

- `dotnet build`: 0 errores, 0 advertencias.
- `dotnet test`: 38 tests pasando (23 dominio, 7 aplicación, 4 arquitectura, 4 integración).
- Migración aplicada: tabla `deliveries` creada en PostgreSQL.
- Persistencia verificada con `SELECT`: `status = Asignada`.