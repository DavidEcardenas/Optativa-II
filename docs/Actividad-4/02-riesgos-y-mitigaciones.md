# Riesgos y mitigaciones — Actividad 4

## Riesgos identificados

| # | Riesgo | Tipo | Mitigación | Evidencia |
|---|---|---|---|---|
| R-01 | El servicio de repartidores no responde | Operativo | El adaptador devuelve `false` → el caso de uso lanza `Courier not available` | Test `Execute_WhenCourierNotAvailable_Throws` |
| R-02 | El servicio de inventario no responde | Operativo | El adaptador devuelve `false` → el caso de uso lanza `Stock not reserved` | Test `Execute_WhenStockNotReserved_Throws` |
| R-03 | Doble asignación por reintento del cliente | Negocio | INV-01 en `Delivery.AssignCourier` | Test `AssignCourier_WhenAlreadyAssigned_Throws` |
| R-04 | Datos personales expuestos en logs | Seguridad | El middleware no loguea el body; solo método y ruta | Revisión de `ExceptionHandlingMiddleware.cs` |
| R-05 | Falla al persistir deja estado inconsistente | Operativo | `SaveAsync` dentro de la transacción del `DbContext`; el controller devuelve 409 | Test `Post_UnknownDelivery_ReturnsNotFound` |
| R-06 | Migraciones no reproducibles | Operativo | Migraciones generadas por `dotnet ef` y versionadas en el repo | `dotnet ef database update` desde cero |

## Mitigaciones detalladas

### R-01 y R-02 — Fallo de servicios externos

Los adaptadores HTTP capturan la respuesta no exitosa y devuelven `false` en lugar de lanzar excepción. El caso de uso traduce ese `false` en un `DomainException` con mensaje claro, que el controller convierte en 409 o 404 según corresponda. En modo demo, los dobles devuelven `true` para permitir probar el flujo sin servicios reales.

### R-03 — Doble asignación

La entidad `Delivery` verifica `CourierId is not null` antes de asignar. Si ya tiene repartidor, lanza `DomainException("Delivery already assigned")`. El controller lo captura y devuelve 409. Es imposible asignar dos repartidores a la misma entrega.

### R-04 — Datos personales en logs

El `ExceptionHandlingMiddleware` solo registra el mensaje de la excepción, no el body de la request ni la dirección del cliente. El `DeliveryAssignedHandler` loguea solo IDs (`DeliveryId`, `CourierId`) y timestamp, que no son PII.

### R-05 — Consistencia al persistir

EF Core envuelve el `SaveAsync` en una transacción implícita. Si falla, no se confirma ningún cambio. El controller devuelve 409 con el mensaje de error, y la entrega conserva su estado original.

### R-06 — Migraciones reproducibles

La carpeta `src/Infrastructure/Persistence/Migrations/` está versionada en el repo. Cualquiera que clone el repo puede aplicar el esquema con un solo comando. No hay cambios manuales al esquema.

## Riesgos de seguridad identificados

| Riesgo | Mitigación actual | Riesgo residual |
|---|---|---|
| Endpoint público sin autenticación | Ninguna en esta entrega | RR-03 |
| Sin rate limiting | Ninguna en esta entrega | RR-04 |
| Sin cifrado en reposo | PostgreSQL por defecto no cifra | Se documenta en ADR-003 |
| Sin cifrado en tránsito | HTTP en local | Se resuelve con HTTPS en producción |