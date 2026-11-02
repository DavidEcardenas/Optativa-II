# Observabilidad

## Señales definidas

| Señal | Tipo | Dónde | Qué permite diagnosticar |
|---|---|---|---|
| `delivery.assign.success` | Contador | Caso de uso | Frecuencia de asignaciones exitosas |
| `delivery.assign.failure` | Contador + etiqueta `reason` | Caso de uso | Motivos de rechazo (stock, courier, ruta, doble asignación) |
| `delivery.assign.duration_ms` | Histograma | Caso de uso | Latencia del flujo |
| `delivery.assigned.event_published` | Contador | InMemoryEventPublisher | Eventos publicados |
| Log `Delivery assigned` | Log estructurado | DeliveryAssignedHandler | Trazabilidad de cada asignación |

## Datos sensibles excluidos

- No se loguea el body de la request.
- No se loguea la dirección del cliente.
- No se loguea el `RequestedBy` completo (solo dominio del correo).
- Los IDs (`DeliveryId`, `CourierId`) sí se loguean porque no son PII.