# Criterios verificables de la decisión

Lista para revisar si el monolito modular responde a las necesidades del sistema. Cada criterio es comprobable y apunta al escenario o requisito que valida.

## Criterios de la estructura

| ID | Criterio verificable | Cómo se comprueba | Valida |
|---|---|---|---|
| CV-01 | Existen los 6 módulos definidos (Pedidos, Inventario, Entregas, Incidencias, Notificaciones, Integraciones) como paquetes separados | Revisión de la estructura de código | ADR-001 |
| CV-02 | Ningún módulo accede a las tablas de otro | Prueba automática de dependencias y revisión del esquema de datos | RNF-09, QS-08 |
| CV-03 | Un cambio en la regla de asignación de repartidores solo modifica el módulo de Entregas | Revisión de un cambio de ejemplo (diff) | QS-08 |

## Criterios de calidad

| ID | Criterio verificable | Cómo se comprueba | Valida |
|---|---|---|---|
| CV-04 | 0 sobreventas con 1.000 intentos simultáneos de reservar la última unidad | Prueba de concurrencia | QS-02, RNF-03 |
| CV-05 | Consulta de estado de pedido con p95 < 2 s a 1.000 pedidos/hora | Prueba de carga | QS-01, RNF-01 |
| CV-06 | Con 3.000 pedidos/hora durante 30 min no se pierde ningún pedido y la ingesta mantiene p95 < 5 s | Prueba de carga con pico | QS-03, RNF-02 |
| CV-07 | Si el servicio de notificaciones falla, los pedidos siguen procesándose y las notificaciones se reintentan | Prueba de falla simulada | QS-04, RNF-04 |
| CV-08 | Si la pasarela no responde, el pedido queda en "pago pendiente" y se reintenta | Prueba de falla simulada | QS-05, RNF-04 |
| CV-09 | El reenvío del mismo pedido no genera duplicados | Prueba de idempotencia | QS-09, RNF-08 |
| CV-10 | Un repartidor no puede ver pedidos que no tiene asignados y el intento queda registrado | Prueba de autorización y revisión del registro | QS-06, RNF-05 |
| CV-11 | Los datos personales viajan y se almacenan cifrados | Revisión de configuración | RNF-05 |
| CV-12 | Tras 10 min sin conexión, la app sincroniza en < 1 min y sin duplicados | Prueba con conexión simulada | QS-07, RNF-07 |
| CV-13 | Una incidencia reportada aparece en el panel de soporte en < 5 s | Prueba de extremo a extremo | QS-10, RF-10 |
| CV-14 | Todo cambio de estado de pedido, stock y entrega queda con fecha, hora y responsable | Revisión del registro de auditoría | RNF-06 |

## Criterios de la decisión

| ID | Criterio verificable | Cómo se comprueba |
|---|---|---|
| CV-15 | El ADR documenta al menos dos alternativas, justificación, riesgos y trade-offs | Revisión del ADR-001 |
| CV-16 | Los disparadores de reevaluación están definidos y se revisan al cerrar cada fase | Revisión del ADR-001 |
| CV-17 | Si algún escenario falla de forma sostenida o se activa un disparador, el ADR se reabre | Revisión de resultados de pruebas |