# Registro de decisiones iniciales y riesgos

## Decisiones tomadas

- **Arquitectura e integración orientada a eventos/APIs:** el sistema central expondrá endpoints REST/Webhooks o eventos para recibir transacciones del ERP/CRM y enviar eventos de estado.
- **Modelo de inventario centralizado:** la plataforma lleva un registro de existencias reservadas y prometidas para LastMile, sincronizándose periódicamente con los almacenes.
- **Flujo unificado de incidencias:** cualquier novedad reportada por el repartidor se refleja inmediatamente en el panel de soporte de cliente sin intermediación manual.

## Riesgos (lo que no se sabe y lo que falta confirmar)

- Se desconoce el volumen máximo de transacciones simultáneas durante los eventos de tráfico alto.
- Se desconoce el nivel de autonomía de los repartidores.
- Falta confirmar los requisitos de cumplimiento legal o reglamentos sobre protección de datos personales de los clientes.
- Falta confirmar los protocolos de contingencia cuando el repartidor pierde conexión a internet en zonas de cobertura muy limitada.