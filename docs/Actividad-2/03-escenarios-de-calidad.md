# Escenarios de calidad

Cada escenario define fuente, estímulo, entorno, respuesta y una **medida concreta** que permite comprobarlo. Las cifras son propuestas de trabajo (ver nota del [catálogo](./01-catalogo-de-requisitos.md)).

| ID | Atributo | Fuente y estímulo | Entorno | Respuesta esperada | Medida |
|---|---|---|---|---|---|
| QS-01 | Rendimiento (RNF-01) | Un cliente o agente de soporte consulta el estado de un pedido | Operación normal, 1.000 pedidos/hora | El sistema devuelve el estado actual | p95 < 2 s |
| QS-02 | Consistencia (RNF-03) | Dos canales intentan reservar la última unidad de un producto al mismo tiempo | Concurrencia simultánea | Una reserva se confirma y la otra se rechaza con aviso de falta de stock | 0 sobreventas en 1.000 intentos simultáneos |
| QS-03 | Capacidad (RNF-02) | Llega un pico de pedidos | 3.000 pedidos/hora durante 30 min | El sistema sigue aceptando pedidos y degrada de forma controlada tareas no críticas | 0 pedidos perdidos; ingesta p95 < 5 s |
| QS-04 | Disponibilidad (RNF-04) | El sistema de notificaciones deja de responder | Operación normal | Los pedidos siguen procesándose; las notificaciones quedan en cola y se reintentan | 0 pedidos bloqueados; notificaciones entregadas < 15 min tras recuperarse el servicio |
| QS-05 | Disponibilidad (RNF-04) | La pasarela de pagos no responde | Operación normal | El pedido queda en estado "pago pendiente de verificación" y se reintenta | 0 pedidos perdidos; reintentos automáticos hasta la verificación |
| QS-06 | Seguridad (RNF-05) | Un repartidor intenta ver pedidos que no tiene asignados | Operación normal | El acceso se rechaza y se registra | 100 % rechazados y auditados; datos personales cifrados en tránsito y en reposo |
| QS-07 | Resiliencia (RNF-07) | Un repartidor pierde conexión 10 minutos durante la ruta | Zona de cobertura limitada | La app conserva telemetría y confirmaciones y las envía al reconectar | Sincronización < 1 min tras reconectar, sin registros duplicados |
| QS-08 | Evolución (RNF-09) | El negocio cambia la regla de asignación de repartidores | Desarrollo | El cambio se hace en un solo módulo sin tocar los demás | Cambios en un solo módulo; pruebas de los otros módulos sin modificar |
| QS-09 | Interoperabilidad (RNF-08) | El ERP reenvía el mismo pedido por un error de red | Operación normal | El sistema reconoce el pedido y no lo duplica | 0 pedidos duplicados |
| QS-10 | Trazabilidad (RF-10, RNF-06) | El repartidor reporta una incidencia de entrega | Operación normal | La incidencia aparece en el panel de soporte sin intervención manual | Visible en < 5 s y con historial de cambios completo |