# Matriz de priorización

Escala: **Alta / Media / Baja** para prioridad y complejidad; **Alto / Medio / Bajo** para impacto.
La prioridad se asigna según la importancia para el negocio (reducir sobreventas, entregas fallidas y carga de soporte) y el impacto sobre la estructura del sistema.

## Requisitos funcionales

| ID | Requisito | Prioridad | Impacto en negocio | Impacto en arquitectura | Complejidad |
|---|---|---|---|---|---|
| RF-01 | Ingesta de pedidos multicanal | Alta | Alto | Alto | Media |
| RF-02 | Estado único del pedido | Alta | Alto | Alto | Media |
| RF-03 | Verificación de pago | Alta | Alto | Medio | Baja |
| RF-04 | Ingesta de productos | Media | Medio | Bajo | Baja |
| RF-05 | Consolidación y sincronización de stock | Alta | Alto | Alto | Alta |
| RF-06 | Reserva de stock | Alta | Alto | Alto | Alta |
| RF-07 | Estado de preparación y empaque | Alta | Medio | Bajo | Baja |
| RF-08 | Ruta y asignación de repartidor | Alta | Alto | Medio | Alta |
| RF-09 | Telemetría y confirmación de entrega | Alta | Alto | Alto | Media |
| RF-10 | Gestión de incidencias | Alta | Alto | Medio | Media |
| RF-11 | Notificaciones al cliente | Media | Medio | Medio | Baja |
| RF-12 | Eventos de estado hacia el ERP/CRM | Media | Medio | Medio | Media |
| RF-13 | Despachos desde tiendas físicas | Media | Medio | Bajo | Media |
| RF-14 | Usuarios y permisos por actor | Alta | Medio | Alto | Media |

## Atributos de calidad

| ID | Atributo | Prioridad | Impacto en negocio | Impacto en arquitectura | Complejidad |
|---|---|---|---|---|---|
| RNF-03 | Consistencia de inventario | Alta | Alto | Alto | Alta |
| RNF-02 | Capacidad ante picos | Alta | Alto | Alto | Alta |
| RNF-08 | Interoperabilidad e idempotencia | Alta | Alto | Alto | Media |
| RNF-04 | Disponibilidad | Alta | Alto | Alto | Media |
| RNF-05 | Seguridad y privacidad | Alta | Alto | Alto | Media |
| RNF-06 | Trazabilidad y auditoría | Alta | Alto | Medio | Media |
| RNF-01 | Rendimiento de consulta | Alta | Alto | Medio | Media |
| RNF-09 | Evolución y mantenibilidad | Alta | Medio | Alto | Media |
| RNF-07 | Resiliencia ante desconexión | Media | Medio | Alto | Alta |
| RNF-10 | Observabilidad | Media | Medio | Medio | Baja |

## Drivers arquitectónicos

De la matriz salen los requisitos con **alto impacto en la arquitectura** que más condicionan la decisión:

1. **RNF-03 Consistencia de inventario** y **RF-06 Reserva de stock**: evitar colisiones y ventas sin stock es el problema central del caso.
2. **RF-02 Estado único** con **RNF-06 Trazabilidad**: la fuente única de verdad es la razón de ser del sistema.
3. **RNF-08 Interoperabilidad**: el sistema vive entre el ERP/CRM, canales de venta, pasarela, geolocalización y notificaciones.
4. **RNF-04 Disponibilidad** y **RNF-02 Capacidad**: la operación de entrega no puede detenerse y el volumen máximo aún es desconocido.
5. **RNF-09 Evolución**: el sistema debe poder crecer por módulos sin reescribirse.
6. **RNF-05 Seguridad**: se manejan datos personales de clientes.