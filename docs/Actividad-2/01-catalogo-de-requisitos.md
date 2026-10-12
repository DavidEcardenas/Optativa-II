## 1. Requisitos funcionales (qué debe hacer el sistema)

| ID | Requisito |
|---|---|
| RF-01 | Recibir pedidos de múltiples canales de venta y del ERP/CRM mediante API REST/webhooks |
| RF-02 | Mantener un estado único del pedido (fuente única de verdad) y permitir consultarlo |
| RF-03 | Consultar a la pasarela de pagos para verificar la transacción del pedido |
| RF-04 | Realizar la ingesta centralizada de productos |
| RF-05 | Consolidar el inventario por nodo y sincronizar/validar el stock con los almacenes |
| RF-06 | Reservar stock en el almacén más cercano, registrando existencias reservadas y prometidas |
| RF-07 | Permitir al administrador de almacén actualizar el estado de preparación y empaque |
| RF-08 | Calcular la ruta óptima con el servicio de geolocalización y asignar la tarea a un repartidor |
| RF-09 | Recibir telemetría de ubicación y confirmación de entrega del repartidor |
| RF-10 | Gestionar incidencias de entrega y reflejarlas de inmediato en el panel de soporte |
| RF-11 | Enviar notificaciones automáticas al cliente sobre el estado de su pedido |
| RF-12 | Publicar eventos de estado hacia el ERP/CRM |
| RF-13 | Soportar despachos desde tiendas físicas |
| RF-14 | Gestionar usuarios y permisos por tipo de actor (cliente, operador, almacén, repartidor, soporte) |

## 2. Requisitos no funcionales (atributos de calidad)


| ID | Atributo | Requisito |
|---|---|---|---|
| RNF-01 | Rendimiento | Consultar el estado de un pedido debe ser rápido |
| RNF-02 | Escalabilidad / capacidad | Soportar la carga normal y picos de tráfico alto |
| RNF-03 | Consistencia de datos | No vender stock que no existe; reserva sin colisiones entre canales |
| RNF-04 | Disponibilidad | La operación no debe detenerse por fallas parciales |
| RNF-05 | Seguridad y privacidad | Proteger datos personales y controlar el acceso por rol |
| RNF-06 | Trazabilidad y auditoría | Registrar cada cambio de estado de pedido, stock y entrega |
| RNF-07 | Resiliencia operativa | Tolerar pérdida de conexión del repartidor |
| RNF-08 | Interoperabilidad | Contratos de API versionados e idempotencia en la recepción de pedidos |
| RNF-09 | Evolución y mantenibilidad | Cambiar reglas de negocio sin tocar toda la aplicación |
| RNF-10 | Observabilidad | Detectar y diagnosticar fallas con rapidez |

## 3. Restricciones

| ID | Restricción | Origen |
|---|---|---|
| RES-01 | Cobranza delegada a pasarelas de pago; LastMile no procesa pagos | Fuera de alcance |
| RES-02 | Facturación contable delegada al ERP | Fuera de alcance |
| RES-03 | No se optimiza flota de larga distancia | Fuera de alcance |
| RES-04 | El ERP y el catálogo/e-commerce ya existen y son el origen de los pedidos | Supuesto |
| RES-05 | Los repartidores usan dispositivos móviles con datos y GPS | Supuesto |
| RES-06 | Integración mediante REST/webhooks/eventos | Decisión inicial |
| RES-07 | Inventario centralizado con sincronización periódica hacia los almacenes | Decisión inicial |
| RES-08 | Normativa de protección de datos por confirmar | A confirmar en la Actividad 1 |