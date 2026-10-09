# Diagrama de contexto del sistema

Fig(Diagrama de contexto del sistema)

## Flujos principales de información

1. **Ingesta y validación:** el ERP/CRM interno o los canales de venta envían el pedido a LastMile. La plataforma consulta la pasarela de pagos para verificar la transacción y reserva stock en el almacén más cercano.
2. **Preparación y asignación:** el administrador del almacén empaqueta el pedido y actualiza el estado. LastMile utiliza el servicio de geolocalización para calcular la ruta óptima y asignar la tarea a un repartidor.
3. **Seguimiento de incidencias:** el repartidor envía telemetría de ubicación y confirmación de entrega. LastMile dispara mensajes a través del sistema de notificaciones al usuario final. Si ocurren desviaciones, el soporte técnico gestionará su solución por la misma plataforma.