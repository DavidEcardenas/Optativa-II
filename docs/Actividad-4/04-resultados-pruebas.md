# Resultados de pruebas — Actividad 4

## Comando ejecutado

```bash
dotnet test
```

## Resultado final

| Suite | Tests | Resultado |
|---|---|---|
| `LastMile.Domain.Tests` | 23 | ✅ Pasan |
| `LastMile.Application.Tests` | 7 | ✅ Pasan |
| `LastMile.Architecture.Tests` | 4 | ✅ Pasan |
| `LastMile.Integration.Tests` | 4 | ✅ Pasan |
| **Total** | **38** | ✅ |

## Evidencia por suite

### Domain.Tests (23 tests)

Prueban las invariantes y transiciones de estado de `Delivery`:

- `AssignCourier_FromPrepared_ChangesStatusToAsignada` — camino feliz.
- `AssignCourier_WhenAlreadyAssigned_Throws` — INV-01.
- `AssignCourier_WhenDelivered_Throws` — INV-06.
- `AssignCourier_FromPendiente_Throws` — transición inválida.
- `MarkAsPrepared_FromPendiente_ChangesStatus` — transición válida.
- `StartRoute_FromAsignada_ChangesStatus` — transición válida.
- `ConfirmDelivery_FromEnRuta_ChangesStatus` — transición válida.
- `ReportIncident_FromEnRuta_ChangesStatus` — transición válida.
- `ResolveIncident_FromConIncidencia_ReturnsToEnRuta` — transición válida.
- `MarkAsFailed_FromConIncidencia_ChangesStatus` — transición válida.
- `AssignCourier_WithEmptyAssignedBy_Throws` — validación de input.
- 5 tests de `DeliveryAddress`.
- 5 tests de `DeliveryWindow`.

### Application.Tests (7 tests)

Prueban el caso de uso con dobles de los puertos:

- `Execute_WithValidInput_AssignsCourierAndPublishesEvent` — camino feliz.
- `Execute_WhenDeliveryAlreadyAssigned_Throws` — INV-01.
- `Execute_WhenStockNotReserved_Throws` — INV-02.
- `Execute_WhenCourierNotAvailable_Throws` — INV-04.
- `Execute_WhenRouteHasNoCapacity_Throws` — INV-03.
- `Execute_WhenDeliveryNotFound_Throws` — 404.
- Tests de los `Fakes` (5 archivos).

### Architecture.Tests (4 tests)

Verifican la dirección de dependencias:

- `Domain_ShouldNotDependOn_Application`.
- `Domain_ShouldNotDependOn_Infrastructure`.
- `Application_ShouldNotDependOn_Infrastructure`.
- Verificación adicional del aislamiento entre capas.

### Integration.Tests (4 tests)

Prueban el endpoint end-to-end con `WebApplicationFactory`:

- `Post_ValidRequest_ReturnsOkAndPersists` — 200 y persistencia.
- `Post_UnknownDelivery_ReturnsNotFound` — 404.
- `Post_EmptyRequestedBy_ReturnsBadRequest` — 400.
- (Test adicional de la factory).

## Prueba end-to-end manual

Además de los tests automatizados, se probó manualmente con `curl`:

| Caso | Comando | Resultado |
|---|---|---|
| Asignación exitosa | `POST /deliveries/{id}/assign-courier` con body válido | 200 + JSON |
| Entrega inexistente | `POST` con ID `00000000-...` | 404 + `{"error":"Delivery not found"}` |
| RequestedBy vacío | `POST` con `"requestedBy":""` | 400 + `{"error":"RequestedBy is required"}` |
| Doble asignación | `POST` repetido | 409 + `{"error":"Delivery already assigned"}` |

## Persistencia verificada

```sql
SELECT id, status, courier_id FROM deliveries;
```

Resultado:

```
                  id                  |  status   |              courier_id              
--------------------------------------+-----------+--------------------------------------
 aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa | Asignada | cccccccc-cccc-cccc-cccc-cccccccccccc
```

## Build

```bash
dotnet build
```

Resultado:

```
Build succeeded.
    0 Warning(s)
    0 Error(s)
```