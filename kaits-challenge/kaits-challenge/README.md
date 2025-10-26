# 🧩 Kaits Challenge – Sistema de Registro y Listado de Pedidos

Proyecto full stack desarrollado con **.NET 8 (Web API)** y **React + TypeScript + TailwindCSS**, 
que permite registrar pedidos de clientes, agregar productos dinámicamente, calcular totales, mostrar listados de pedidos registrados y aplicar validaciones tanto en el servidor como en el cliente.

---

## 🚀 Instrucciones para ejecutar el proyecto

### 🖥️ Requisitos previos

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/)
- Un IDE compatible: **Visual Studio Code**, **Rider**, o **Visual Studio 2022**
- Base de datos: **SQL Server LocalDB / SQLite**

---
## 🚀 Puesta en marcha

### 1) SQL Server
Ejecuta en este orden:
```
sql/create_db.sql
sql/seed_clientes.sql
sql/seed_productos.sql

### 🧱 Backend – (Kaits.WebApi)
1. Configura la cadena en `appsettings.json` 

2. Abrir una terminal en la carpeta:
   ```bash
   cd backend/Kaits.WebApi
   ```

3. Restaurar dependencias:
   ```bash
   dotnet restore
   ```

4. Aplicar las migraciones de base de datos:
   ```bash
   dotnet ef database update
   ```

5. Ejecutar el proyecto:
   ```bash
   dotnet run
   ```

   Por defecto:
   - 🔹 API REST: https://localhost:5001
   - 🔹 Swagger UI: https://localhost:5001/swagger

---

### 🌐 Frontend – (React + TypeScript + Vite + TailwindCSS)

1. Abrir una nueva terminal y posicionarse en:
   ```bash
   cd frontend
   ```

2. Instalar dependencias:
   ```bash
   npm install
   ```

3. Ejecutar el entorno de desarrollo:
   ```bash
   npm run dev
   ```

4. Abrir en navegador: !IMPORTANTE
   👉 http://localhost:5173

---

## 🏗️ Arquitectura elegida y decisiones técnicas

El proyecto sigue los principios de **Clean Architecture** con una implementación **CQRS Light** y **MediatR**, asegurando una clara separación de responsabilidades y mantenibilidad a largo plazo.

### 🔹 Estructura general del backend

| Capa | Descripción |
|------|--------------|
| **Domain** | Contiene las entidades principales, DTOs y reglas de negocio puras. |
| **Application** | Casos de uso (Commands y Queries) implementados mediante Mediator. Incluye validaciones con FluentValidation. |
| **Infrastructure** | Implementación de persistencia (EF Core), repositorios y configuración de base de datos. |
| **WebApi** | Capa de presentación (controladores), configuración de dependencias, manejo de excepciones y Swagger. |

### 🔹 Estructura del frontend

| Carpeta | Descripción |
|----------|--------------|
| **/src/components** | Componentes reutilizables como formularios, tablas y modales. |
| **/src/api** | Cliente Axios centralizado (`client.ts`) para comunicación con el backend. |
| **/src/pages** | Vistas principales (Registrar Pedido, Listado de Pedidos). |
| **/src/styles** | Configuración de TailwindCSS. |

---

### ⚙️ Decisiones técnicas clave

- **MediatR + CQRS Light:** Cada acción (crear pedido, obtener pedidos, etc.) se maneja mediante *Handlers* independientes para mayor modularidad.
- **FluentValidation:** Validaciones robustas de reglas de negocio antes de ejecutar los comandos.
- **Entity Framework Core:** Persistencia basada en ORM con migraciones, relaciones (`Order`, `OrderItem`, `Customer`, `Product`).
- **React Hook Form + Zod:** Validaciones reactivas en el frontend sincronizadas con los esquemas del backend.
- **TailwindCSS:** Diseño moderno, responsive y con componentes limpios.
- **Eventos personalizados (`orders:refresh`):** Permiten actualizar el listado en tiempo real tras registrar un nuevo pedido.
- **Manejo de errores centralizado:** Middleware global para excepciones, con logs y códigos HTTP consistentes.

---

## 📦 Paquetes NuGet y librerías principales utilizadas

### 🧩 Backend (.NET)

| Paquete | Descripción |
|----------|--------------|
| `MediatR` | Implementación del patrón Mediator para CQRS. |
| `FluentValidation` | Validaciones declarativas de entidades y comandos. |
| `Microsoft.EntityFrameworkCore` | ORM para acceso a datos y migraciones. |
| `Microsoft.EntityFrameworkCore.Tools` | Soporte de comandos EF Core. |
| `Swashbuckle.AspNetCore` | Generación automática de Swagger UI. |
| `Microsoft.Extensions.Logging` | Manejo de logs centralizado. |

---

### 💻 Frontend (React + TypeScript)

| Librería | Descripción |
|-----------|--------------|
| `react-hook-form` | Manejo eficiente de formularios. |
| `zod` | Validación tipada de datos. |
| `@hookform/resolvers` | Integración entre React Hook Form y Zod. |
| `axios` | Cliente HTTP para comunicación con el backend. |
| `tailwindcss` | Framework CSS utilitario y responsive. |
| `postcss` / `autoprefixer` | Compatibilidad de estilos en navegadores. |
| `vite` | Bundler moderno y rápido para desarrollo. |

---

## 🧠 Buenas prácticas implementadas

| # | Práctica | Implementación |
|---|-----------|----------------|
| **1** | **Arquitectura limpia y organizada (capas, DTOs, validaciones)** | Proyecto estructurado en capas Domain, Application, Infrastructure y WebApi; DTOs bien definidos. |
| **2** | **Aplicación de patrones (CQRS Light, Mediator)** | Commands y Queries desacoplados mediante `MediatR`. |
| **3** | **Buen uso de Entity Framework Core** | Migrations, relaciones y tracking optimizado. |
| **4** | **Validaciones de negocio en servidor (FluentValidation)** | Validaciones a nivel de Application antes de persistir. |
| **5** | **Manejo adecuado de excepciones y logs** | Middleware global + Logging en todas las capas. |
| **6** | **Documentación clara (README)** | Documento explicativo y guías de ejecución. |
| **7** | **Tests unitarios e integración** | Proyecto `Kaits.Tests` con pruebas de validación de negocio y creación de pedidos. |
| **8** | **Separación clara entre dominio y presentación** | Backend independiente del frontend, comunicación por REST. |
| **9** | **Uso de control de versiones (Git)** | Estructura de commits limpia: `feat`, `fix`, `refactor`, `docs`. |
| **10** | **Contenedores (opcional)** | Preparado para integración futura con `Dockerfile` y `docker-compose.yml`. |

---

## 🧪 Tests y validaciones

El proyecto incluye pruebas en el módulo `Kaits.Tests`:
- Validaciones de negocio (cliente requerido, productos válidos, totales correctos).
- Pruebas de integración con base de datos InMemory.
- Validación de los `CommandHandler` para creación de pedidos.

---

## 🧭 Mejoras que se pueden aplicar

- Implementar autenticación JWT.
- Contenedorización con Docker Compose.
- Paginación y filtrado avanzado en el listado de pedidos.
- Auditoría de cambios y registros de usuario.

---

## 👨‍💻 Autor

**Kaits Challenge – Clean Architecture Full Stack Project**  
Desarrollado aplicando principios de arquitectura limpia, buenas prácticas y separación de responsabilidades entre capas.  
Frontend moderno y responsive, backend escalable y mantenible.

