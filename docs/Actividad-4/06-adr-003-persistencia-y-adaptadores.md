# ADR-003: Persistencia y adaptadores del módulo Entregas

| Campo | Valor |
|---|---|
| Estado | Aceptada |
| Fecha | 2026-11-02 |
| Decisores | Autor del proyecto |
| Relacionado con | ADR-001, ADR-002, Actividad 4 |

## Contexto

La Actividad 3 definió el dominio del módulo Entregas y sus puertos, pero no implementó persistencia ni adaptadores externos reales. La Actividad 4 pide un vertical slice ejecutable que conecte entrada, dominio, persistencia e integración sin romper los límites del diseño.

El contexto técnico es:

- Monolito modular con PostgreSQL (ADR-001).
- Dominio protegido con `Delivery` como aggregate root (ADR-002).
- Puertos definidos por la Aplicación: `IDeliveryRepository`, `ICourierAvailabilityPort`, `IRouteCapacityPort`, `IStockReservationPort`, `IEventPublisher`.
- Servicios externos de repartidores, rutas y órdenes no implementados aún.

## Decisión

Se adopta la siguiente estrategia de implementación:

### 1. Persistencia con EF Core + PostgreSQL

- `LastMileDbContext` con `DbSet<Delivery>`.
- `DeliveryConfiguration` mapea la entidad con `OwnsOne` para `DeliveryAddress` y `DeliveryWindow`.
- Los IDs tipados (`DeliveryId`, `OrderId`, `CourierId`) se mapean con `HasConversion`.
- El enum `DeliveryStatus` se mapea como `string` para legibilidad en la BD.
- `EfDeliveryRepository` implementa `IDeliveryRepository`.

### 2. Migraciones reproducibles

- Generadas con `dotnet ef migrations add`.
- Versionadas en `src/Infrastructure/Persistence/Migrations/`.
- Aplicables con `dotnet ef database update` desde cero.
- No hay cambios manuales al esquema.

### 3. Adaptadores externos HTTP con modo demo

- `HttpCourierAvailabilityAdapter`, `HttpRouteCapacityAdapter`, `HttpStockReservationAdapter` implementan los puertos.
- Usan `IHttpClientFactory` con `BaseAddress` configurado en `appsettings.json`.
- **Modo demo**: si `ExternalApis:Couriers` o `ExternalApis:Orders` no están configurados, la API usa dobles (`DemoCourierAvailabilityPort`, `DemoRouteCapacityPort`, `DemoStockReservationPort`) que devuelven `true`.
- Los adaptadores HTTP reales se activan automáticamente cuando se configuran las URLs.

### 4. Publicador de eventos en memoria

- `InMemoryEventPublisher` implementa `IEventPublisher`.
- Los eventos se guardan en una `ConcurrentQueue<object>`.
- `DeliveryAssignedHandler` consume el evento y loguea.
- Se anota como riesgo residual RR-01; se reemplazará por RabbitMQ.

### 5. Pruebas de integración con `WebApplicationFactory`

- `LastMileWebAppFactory` reemplaza el `DbContext` por `UseInMemoryDatabase`.
- Reemplaza los puertos externos por dobles.
- Prueba el endpoint end-to-end con `HttpClient`.

### 6. Fitness function de arquitectura

- `LayerDependencyTests` usa NetArchTest para verificar que:
  - `Domain` no dependa de `Application` ni `Infrastructure`.
  - `Application` no dependa de `Infrastructure`.
- Se ejecuta en cada `dotnet test`.

## Alternativas consideradas

| Alternativa | Resultado | Motivo |
|---|---|---|
| EF Core + PostgreSQL | **Elegida** | Consistente con ADR-001, migraciones reproducibles, integración con .NET |
| Dapper + SQL manual | Descartada | Más control pero más código; EF Core ya justificado |
| Repositorios genéricos | Descartada | Acoplan el dominio a un patrón que no necesita |
| Mocks de EF Core | Descartada | No prueban el mapeo real; se prefiere `UseInMemoryDatabase` en tests de integración |
| RabbitMQ en esta entrega | Descartada | Agrega complejidad de infraestructura; se anota como próximo paso |
| Polly en esta entrega | Descartada | Se anota como riesgo residual RR-02; se agrega en próxima iteración |

## Consecuencias

### Positivas

- Flujo end-to-end ejecutable y verificable.
- Mapeo EF Core validado con tests de integración.
- Migraciones reproducibles y versionadas.
- El dominio sigue aislado: no conoce EF Core ni HTTP.
- El modo demo permite probar sin servicios externos.
- La fitness function verifica automáticamente la dirección de dependencias.

### Negativas / trade-offs aceptados

- El publicador en memoria no es productivo (RR-01).
- Los adaptadores HTTP no tienen reintentos (RR-02).
- El modo demo puede enmascarar problemas de integración real.
- La base de datos en memoria de los tests no valida constraints de PostgreSQL.
- No hay autenticación ni rate limiting (RR-03, RR-04).

## Riesgos y mitigaciones

| Riesgo | Mitigación |
|---|---|
| El modo demo se queda en producción | Documentar en `appsettings.json` que `ExternalApis` debe estar configurado en producción |
| El publicador en memoria pierde eventos | Reemplazar por RabbitMQ (próximo paso priorizado) |
| Los adaptadores HTTP fallan sin reintentos | Agregar Polly (próximo paso priorizado) |
| La entidad `Delivery` crece demasiado | Revisar límites del agregado en cada iteración |
| Los tests de integración usan InMemory y no PostgreSQL | Agregar tests con Testcontainers en próxima iteración |

## Verificación

| Criterio | Cómo se comprueba |
|---|---|
| El flujo end-to-end funciona | `Post_ValidRequest_ReturnsOkAndPersists` + prueba manual con `curl` |
| La persistencia es correcta | `SELECT` en PostgreSQL muestra `status = Asignada` |
| Las migraciones son reproducibles | `dotnet ef database update` desde BD vacía |
| El dominio no depende de infraestructura | `LayerDependencyTests` en verde |
| Los casos de error funcionan | 404, 400, 409 probados con `curl` y tests |

## Disparadores para reevaluar la decisión

1. Se necesita procesar más de 3.000 pedidos/hora → reevaluar el publicador de eventos y la escalabilidad.
2. Aparecen servicios externos reales → reemplazar el modo demo por los adaptadores HTTP con Polly.
3. Se requiere trazabilidad distribuida → agregar OpenTelemetry y correlation IDs.
4. El equipo crece y necesita desplegar módulos por separado → reevaluar el ADR-001 y extraer servicios.