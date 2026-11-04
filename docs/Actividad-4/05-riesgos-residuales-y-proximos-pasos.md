# Riesgos residuales y próximos pasos — Actividad 4

## Riesgos residuales

| # | Riesgo | Por qué queda abierto | Impacto | Prioridad de resolución |
|---|---|---|---|---|
| RR-01 | El publicador de eventos es en memoria | No persiste; si el proceso cae, el evento se pierde | Alto | Alta |
| RR-02 | Los adaptadores HTTP no tienen reintentos | Un fallo transitorio se traduce en rechazo | Medio | Alta |
| RR-03 | No hay autenticación | El endpoint es público | Alto | Alta |
| RR-04 | No hay rate limiting | Un cliente podría saturar el endpoint | Medio | Media |
| RR-05 | No hay pruebas de carga | No se validó RNF-02 (3.000 pedidos/hora) | Medio | Media |
| RR-06 | Datos en tránsito sin cifrar (HTTP) | En local no hay HTTPS configurado | Medio | Alta (en producción) |
| RR-07 | PostgreSQL sin cifrado en reposo | Configuración por defecto | Alto | Alta (en producción) |

## Detalle de cada riesgo residual

### RR-01 — Publicador en memoria

**Qué pasa:** el `InMemoryEventPublisher` guarda los eventos en una `ConcurrentQueue`. Si el proceso cae antes de que otro módulo los consuma, se pierden.

**Impacto:** Notificaciones e Incidencias no reciben el evento `DeliveryAssigned`.

**Próximo paso:** reemplazar por RabbitMQ o Azure Service Bus. El caso de uso no cambia porque solo depende del puerto `IEventPublisher`. Se agrega un adaptador `RabbitEventPublisher` en `/src/Infrastructure/Events/`.

**Criterio de aceptación:** el evento se persiste en una cola durable; si el consumidor está caído, el evento se entrega al reconectar.

### RR-02 — Adaptadores HTTP sin reintentos

**Qué pasa:** los adaptadores `HttpCourierAvailabilityAdapter`, `HttpRouteCapacityAdapter` y `HttpStockReservationAdapter` hacen una sola llamada. Si el servicio externo falla de forma transitoria, el flujo entero falla.

**Impacto:** asignaciones rechazadas por fallos de red temporales.

**Próximo paso:** agregar Polly con política de reintentos exponenciales (3 intentos, 2s de espera inicial) y circuit breaker.

**Criterio de aceptación:** un fallo transitorio se resuelve en el segundo o tercer intento sin afectar al usuario.

### RR-03 — Sin autenticación

**Qué pasa:** el endpoint `POST /deliveries/{id}/assign-courier` es público. Cualquiera puede llamarlo.

**Impacto:** un atacante podría asignar entregas arbitrariamente.

**Próximo paso:** agregar JWT con roles (RF-14). Solo el rol `WarehouseAdmin` puede asignar repartidores.

**Criterio de aceptación:** una petición sin token JWT válido devuelve 401; un token sin el rol correcto devuelve 403.

### RR-04 — Sin rate limiting

**Qué pasa:** no hay límite de peticiones por IP o por usuario.

**Impacto:** un cliente malicioso podría saturar el endpoint.

**Próximo paso:** agregar `AspNetCoreRateLimit` o el rate limiter nativo de .NET 8 con política de 100 peticiones por minuto por IP.

**Criterio de aceptación:** al superar el límite, la API devuelve 429 Too Many Requests.

### RR-05 — Sin pruebas de carga

**Qué pasa:** no se validó que la API soporte 3.000 pedidos/hora (RNF-02).

**Impacto:** no hay evidencia de que el sistema escale ante picos.

**Próximo paso:** pruebas de carga con k6 o JMeter contra el endpoint, midiendo p95 de latencia y throughput.

**Criterio de aceptación:** 3.000 pedidos/hora durante 30 minutos sin pérdida de peticiones y con p95 < 5s.

### RR-06 y RR-07 — Cifrado

**Qué pasa:** en local, la API usa HTTP y PostgreSQL no cifra en reposo.

**Impacto:** en producción, datos personales viajarían sin cifrar.

**Próximo paso:** configurar HTTPS con certificado TLS en el API, y cifrado en reposo de PostgreSQL (TDE o cifrado de disco).

**Criterio de aceptación:** todas las peticiones viajan por HTTPS; la base de datos tiene cifrado en reposo habilitado.

## Próximos pasos ordenados por prioridad

| Prioridad | Acción | Riesgo que resuelve |
|---|---|---|
| 1 | Agregar JWT y roles | RR-03 |
| 2 | Reemplazar `InMemoryEventPublisher` por RabbitMQ | RR-01 |
| 3 | Agregar Polly a los adaptadores HTTP | RR-02 |
| 4 | Configurar HTTPS | RR-06 |
| 5 | Agregar rate limiting | RR-04 |
| 6 | Correr pruebas de carga con k6 | RR-05 |
| 7 | Implementar `StartDeliveryUseCase`, `ConfirmDeliveryUseCase`, `ReportIncidentUseCase` | — |
| 8 | Configurar cifrado en reposo de PostgreSQL | RR-07 |

## Riesgos del diseño que siguen vigentes

Los riesgos del ADR-001 y ADR-002 siguen siendo válidos:

- Los módulos podrían acoplarse con el tiempo (mitigado con `Architecture.Tests`).
- El volumen podría superar la capacidad del monolito (disparador de reevaluación definido).
- La entidad `Delivery` podría crecer demasiado si se agregan más estados (revisar en cada iteración).

## Conclusión

El vertical slice está funcional y validado. Los riesgos residuales son conocidos, acotados y tienen mitigaciones definidas. Ninguno bloquea el uso del flujo en un entorno controlado. Los próximos pasos están priorizados y son incrementales: cada uno se puede implementar sin reescribir el dominio ni el caso de uso, gracias a los puertos definidos en la Actividad 3.