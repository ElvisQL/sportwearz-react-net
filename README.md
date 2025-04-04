# SportWearz Backend API 🛠️⚡

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Azure](https://img.shields.io/badge/Azure-Cloud-0078D4?logo=microsoftazure)](https://azure.microsoft.com/)
![Database Status](https://img.shields.io/badge/Database-Normalization_In_Progress-orange)

Backend para e-commerce deportivo desarrollado en **.NET 8**, con arquitectura pragmática enfocada en rapidez de desarrollo. Conectado a frontend React (https://github.com/ElvisQL/sportwearz-react-net).

> **⚠️ Nota importante**: API diseñada con fines prácticos/educativos. No sigue estrictamente principios REST estándar.

---

## 🏗️ Estructura del Proyecto

```bash
SportWearz.API/
├── WebApplication/ # Controladores y configuración principal
├── Modelo/         # Entidades de base de datos (Data First inicial)
├── DTO/            # Objetos de transferencia para requests/responses
├── Repositorio/    
│   ├── Generico/   # Repositorio base con CRUD genérico
│   └── Ventas/     # Lógica específica de manejo de ventas
├── Servicio/       # Capa de negocio (validaciones, reglas)
└── Utilidades/     # Helpers (manejo de JWT, extensiones)
```
## 🧠 Arquitectura Key Points

### Enfoque Data First (Inicial)
- Se generaron modelos automáticamente desde la DB existente usando Entity Framework.

- Problema actual: Modelos no normalizados (ej: redundancia en tablas de usuarios).

- En progreso: Refactorización manual de modelos para normalización DB.

### Repositorios
- Repositorio Generico
  ```rb
  public interface IGenericRepository<T> where T :class{
    IQueryable<T> Consultar(Expression<Func<T,bool>>? filtro = null);
    Task<T> Crear(T modelo);
    Task<bool> Editar(T modelo);
    Task<bool> Eliminar(T modelo);
    Task<List<Category>> GetCategoriesByIdsAsync(List<int> categoryIds);}
  ```

### Servicios y Controladores
- Flujo tipico: Controlador → Servicio → Repositorio → DB
- Respuestas estandarizadas: Todos los endpoints retornan ActionResult < ResponseDTO >
```rb
 public class ResponseDTO<T>
 {
     public T? Response { get; set; }
     public bool ? Success { get; set; }
     public string? Message { get; set; }

 }
```

### Endpoints no RESTful
- Se priorizó simplicidad sobre convenciones REST:
  - Verbos en rutas: Ej: GET /api/Producto/GetActiveProducts

  - Parámetros complejos vía POST body incluso en GET (por practicidad).

  - Códigos de estado HTTP simplificados (siempre 200 OK + detalle en ResponseDto).

---
## 🚧 Mejoras Pendientes
- Normalizar base de datos (eliminar redundancias)

- Implementar patron CQRS para queries complejas

- Añadir logging centralizado (Serilog)

- Mejorar manejo de errores (excepciones custom)
## 📘 Documentacion
Para ver la documentacion ir a https://sportwearzapp.azurewebsites.net/swagger/index.html
