# Grades.Web

Aplicación ASP.NET Core MVC (.NET 10) para el mantenimiento de estudiantes y sus notas.
Usa Dapper y stored procedures sobre SQL Server.

## Requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)

## Cómo ejecutar

1. Levantar la base de datos (crea `GradesDB`, las tablas, los datos de prueba y los stored procedures):

   ```bash
   docker compose up -d
   ```

   La primera vez demora un poco porque descarga la imagen de SQL Server.
   Para confirmar que el script terminó bien:

   ```bash
   docker compose logs db-init
   ```

2. Ejecutar la web:

   ```bash
   dotnet run --project Grades.Web --launch-profile http
   ```

3. Abrir <http://localhost:5231>

## Base de datos

| Dato       | Valor             |
|------------|-------------------|
| Servidor   | `localhost,1433`  |
| Base       | `GradesDB`        |
| Usuario    | `sa`              |
| Contraseña | `GradesDemo_2026` |

La cadena de conexión está en `Grades.Web/appsettings.json` y el script completo en `Grades.Web/Script-DB.sql`.

Si se modifica el script, se vuelve a aplicar con:

```bash
docker compose up db-init
```

Para borrar todo y empezar con los datos de prueba originales:

```bash
docker compose down -v
docker compose up -d
```

## Estructura del proyecto

| Carpeta         | Contenido                                                        |
|-----------------|------------------------------------------------------------------|
| `Models`        | Clases `Student` y `Grade` con sus validaciones (DataAnnotations) |
| `Repositories`  | Acceso a datos con Dapper; cada método llama a un stored procedure |
| `Controllers`   | Acciones `async` que usan los repositories                        |
| `Views`         | Vistas Razor con `@model`, `_Layout` y vistas parciales para las filas |
