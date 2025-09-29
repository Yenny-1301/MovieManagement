# 🎬 MovieManagement

Proyecto de práctica desarrollado en **.NET 8** para la gestión de películas y usuarios.  
Incluye autenticación con **JWT** y manejo básico de **roles** (`Admin` y `User`).

---

## 🚀 Tecnologías utilizadas
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Entity Framework Core](https://learn.microsoft.com/ef/core/)
- [SQL Server](https://www.microsoft.com/es-es/sql-server/)  
  (o puedes adaptar la cadena de conexión a MySQL si lo prefieres)
- [Swagger / OpenAPI](https://swagger.io/tools/open-source/open-api/)
- [AutoMapper](https://automapper.org/)
- [JWT Bearer Authentication](https://learn.microsoft.com/aspnet/core/security/authentication/jwt)

---

## ⚙️ Configuración inicial

1. **Clonar el repositorio**
   ```bash
   git clone git clone https://github.com/Yenny-1301/MovieManagement.git cd MovieManagement
2. **Instalar el SDK de .NET 8**
	
    Descarga e instala el SDK desde: 	 
    https://dotnet.microsoft.com/download/dotnet/8.0
    Verifica la instalación:
    ```bash
    dotnet --version
1. **Instalar los paquetes NuGet necesarios**

    Desde la terminal, en la carpeta del proyecto principal, ejecuta:
    ```bash
    dotnet restore
    ```
    Esto descargará todos los paquetes listados en el proyecto (`.csproj`).  
    Si necesitas instalar manualmente, asegúrate de tener:

    - `Microsoft.EntityFrameworkCore`
    - `Microsoft.EntityFrameworkCore.SqlServer`
    - `Microsoft.EntityFrameworkCore.Tools`
    - `Swashbuckle.AspNetCore`
    - `AutoMapper.Extensions.Microsoft.DependencyInjection`
    - `Microsoft.AspNetCore.Authentication.JwtBearer`

4. **Configurar la cadena de conexión a la base de datos**
    En `appsettings.json`, ajusta la cadena de conexión según tu entorno:
    ```json
    "ConnectionStrings": {
        "DefaultConnection": "Server=TU_SERVIDOR;Database=MovieDB;User Id=TU_USUARIO;Password=TU_CONTRASEÑA;"
    }
    ```
5. **Aplicar migraciones y crear la base de datos**
    Desde la terminal, en la carpeta del proyecto principal, ejecuta:
    ```bash
    dotnet ef database update
    ```
    Esto creará la base de datos y las tablas según las migraciones definidas.
    > Si es la primera vez, puedes crear la migración inicial con:
    > ```bash
    > dotnet ef migrations add InitialCreate
    > dotnet ef database update
    > ```
6. **Configurar JWT**
    En `appsettings.json`, ajusta la sección de JWT:
    ```json
    "Jwt": {
        "Key": "TU_CLAVE_SECRETA_AQUI",
        "Issuer": "TuEmisor",
        "Audience": "TuAudiencia",
        "DurationInMinutes": 60
    }
    ```
7. **Ejecutar la aplicación**
    Desde la terminal, en la carpeta del proyecto principal, ejecuta:
    ```bash
    dotnet run
    ```
8. **Acceder a Swagger**
    Abre tu navegador y ve a:
    ```
    http://localhost:5000/swagger
    ```
    Aquí podrás ver y probar los endpoints de la API. 
    > El puerto puede variar según tu configuración.  
    > Aquí podrás ver y probar todos los endpoints de la API.

---

## 👤 Roles Iniciales

Los roles `Admin` y `User` se crean automáticamente al iniciar la aplicación (ver `OnModelCreating` en el contexto de datos).

---

## ✨ Funcionalidades Principales

- CRUD de Películas
- Registro y autenticación de usuarios
- Asignación de roles (Admin, User)
- Documentación de endpoints con Swagger

---
## 📂 Estructura del proyecto
 ```bash 
MovieManagement/
┣ Controllers/       # Endpoints de la API
┣ DTOs/              # Objetos de transferencia de datos
┣ Entities/          # Entidades (User, Role, Movie, etc.)
┣ Data/              # DbContext y configuración de EF Core
┣ Services/          # Lógica de negocio
┣ Middleware/        # Manejo de excepciones y seguridad
┣ Mapping/           # Configuración de AutoMapper
┣ Migrations/        # Migraciones de la base de datos
┣ Program.cs         # Configuración de la app
┗ appsettings.json   # Configuración (BD, JWT, etc.)
