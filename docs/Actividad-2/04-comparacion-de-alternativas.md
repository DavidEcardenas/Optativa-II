# Comparación de alternativas estructurales

Se comparan tres alternativas frente a los drivers de la [matriz de priorización](./02-matriz-de-priorizacion.md).

## Alternativas

### Opción A: Monolito modular con límites claros

Una sola aplicación desplegable, dividida en módulos con responsabilidades y datos propios: **Pedidos, Inventario, Entregas, Incidencias, Notificaciones e Integraciones**. Los módulos se comunican por interfaces y eventos internos, nunca accediendo a los datos de otro módulo.

- **Ventajas:** rápido de construir; baja complejidad operativa; la reserva de stock puede usar una transacción local, lo que favorece la consistencia; trazabilidad y mantenimiento simples.
- **Desventajas:** puede volverse una base de código acoplada si los límites no se respetan; un crecimiento fuerte puede exigir refactorización o extraer servicios más adelante.

### Opción B: Arquitectura distribuida por dominios y servicios

Un servicio por dominio (pedidos, inventario, entregas, incidencias, notificaciones), con base de datos propia, comunicados por API y un bus de eventos.

- **Ventajas:** separa mejor inventario, entrega y soporte; permite escalar y desplegar cada parte por separado; facilita la integración con varios sistemas externos.
- **Desventajas:** mayor complejidad operativa (despliegue, monitoreo, red); consistencia eventual entre servicios, que complica la reserva de stock; requiere más infraestructura y coordinación; riesgo de sobrediseño si el volumen no lo exige.

### Opción C: Monolito por capas tradicional

Una sola aplicación organizada en capas técnicas (presentación, lógica, datos) sin separación por dominio.

- **Ventajas:** el más simple y rápido de iniciar; consistencia directa.
- **Desventajas:** sin límites por dominio, el acoplamiento crece rápido; los cambios de reglas tocan varias capas; evoluciona peor.

## Criterios y pesos

Puntaje de 1 (peor) a 5 (mejor) en cada criterio. Los pesos reflejan los drivers: consistencia de inventario y operación simple pesan más porque atacan el problema central y el contexto de equipo y volumen aún incierto.

| Criterio | Peso | A: Monolito modular | B: Servicios distribuidos | C: Monolito por capas |
|---|---|---|---|---|
| Consistencia de inventario (RNF-03) | 20 % | 5 | 2 | 5 |
| Complejidad (mayor puntaje = más simple) | 15 % | 4 | 2 | 5 |
| Costo (mayor puntaje = más barato) | 15 % | 5 | 2 | 5 |
| Rapidez de implementación | 10 % | 5 | 2 | 5 |
| Facilidad de evolución (RNF-09) | 15 % | 4 | 5 | 2 |
| Riesgo operativo (mayor puntaje = menor riesgo) | 15 % | 4 | 2 | 3 |
| Escalabilidad y aislamiento de fallas | 10 % | 3 | 5 | 2 |
| **Total ponderado** | 100 % | **4,35** | **2,75** | **3,95** |

Cálculo de A: 5×0,20 + 4×0,15 + 5×0,15 + 5×0,10 + 4×0,15 + 4×0,15 + 3×0,10 = 4,35.

> Los puntajes son una valoración cualitativa razonada, no una medición. Sirven para hacer explícitos los criterios y poder discutirlos.

## Resultado

La **Opción A** obtiene el mayor puntaje. Gana frente a C porque evoluciona mejor (límites por dominio) con un costo de arranque casi igual, y frente a B porque resuelve mejor la consistencia del inventario y evita complejidad operativa que hoy no está justificada: el volumen máximo es desconocido y no hay evidencia de que se necesiten servicios independientes.

La Opción B queda como **camino de evolución**: la separación por módulos de A permite extraer un servicio concreto si aparecen los disparadores definidos en el ADR.