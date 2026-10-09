# Análisis del problema

**Nombre del producto:** LastMile, sistema operativo logístico.

## Problema inicial

Descentralización operativa e ineficiencia en el flujo de entrega al cliente final. La recepción de pedidos multicanal junto con la gestión de inventarios distribuidos sin un núcleo unificado genera pérdida de trazabilidad en tiempo real, colisiones de inventario, retrasos en rutas y una alta tasa de entregas fallidas.

## Usuarios operativos

Usuarios finales, operadores de tienda/comercio, administradores de almacén, repartidores y agentes de soporte al cliente.

## Objetivos del sistema

1. Centralizar los estados de pedidos provenientes de múltiples canales de ventas.
2. Proveer visibilidad de inventario consolidado en tiempo real de los nodos distribuidos.
3. Automatizar la asignación y seguimiento de las rutas de LastMile.
4. Reducir un 35% los errores operativos por duplicación e inconsistencia de datos.

## Alcance

**Dentro de alcance:** ingesta centralizada de productos, sincronización/validación de stock por nodo, asignación y seguimiento de repartidores, gestión de incidencias de entrega y notificaciones automáticas al cliente.

**Fuera del alcance:** el procesamiento nativo de cobranza (se delega a pasarelas de pago), la facturación contable compleja (se delega al ERP) y la optimización avanzada de flota de larga distancia.

## ¿Qué ocurre hoy?

La operación actual funciona mediante silos de información. Cada canal de venta opera de manera independiente respecto al stock disponible. La gestión de entregas se coordina de forma manual o a través de herramientas no integradas (hojas de cálculo, chats de mensajería instantánea) entre almacén, repartidor y centro de soporte.

## ¿Por qué es un problema?

La falta de una "fuente única de verdad" imposibilita conocer el estado real de un paquete en un momento determinado. No existe visibilidad unificada de los inventarios distribuidos, lo que provoca ventas sin stock disponible real.

## ¿Qué consecuencias tiene?

- Costos operativos elevados por reprocesamiento de los pedidos e intentos de entrega fallidos por inconsistencia en direcciones o tiempos de llegada.
- Insatisfacción de clientes: clientes sin información clara sobre la llegada de sus compras.
- Sobrecarga en soporte: los agentes invierten tiempo llamando a los almacenes y repartidores para ubicar los paquetes manualmente.