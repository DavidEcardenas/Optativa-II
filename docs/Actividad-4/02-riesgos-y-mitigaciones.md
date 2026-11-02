# Riesgos y mitigaciones

| # | Riesgo | Tipo | Mitigación | Evidencia |
|---|---|---|---|---|
| R-01 | El servicio de repartidores no responde | Operativo | El adaptador devuelve `false` → el caso de uso lanza "Courier not available" | Test QS-ASG-05 |
| R-02 | El stock no está reservado | Negocio | Precondición INV-02 en el caso de uso | Test QS-ASG-03 |
| R-03 | Doble asignación por reintento del cliente | Negocio | INV-01 en `Delivery.AssignCourier` | Test QS-ASG-02 |
| R-04 | Datos personales expuestos en logs | Seguridad | El middleware no loguea el body; solo método y ruta | Revisión de ExceptionHandlingMiddleware |
| R-05 | Falla al persistir deja estado inconsistente | Operativo | `SaveAsync` dentro de try; el controller devuelve 409 | Test Post_UnknownDelivery_ReturnsNotFound |
| R-06 | Migraciones no reproducibles | Operativo | Migraciones generadas por `dotnet ef` versionadas en el repo | `dotnet ef database update` desde cero |