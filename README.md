# 🎬 MovieManagement

Proyecto de práctica desarrollado en **.NET 8** para la gestión de películas y usuarios.  
Incluye autenticación con **JWT** y manejo básico de **roles** (`Admin` y `User`).

---

## 🚀 Tecnologías utilizadas
- [.NET 8](https://dotnet.microsoft.com/)
- [Entity Framework Core](https://learn.microsoft.com/ef/core/)
- [SQL Server](https://www.microsoft.com/es-es/sql-server/) (conexión mediante `Microsoft.EntityFrameworkCore.SqlServer`)
- Swagger

---

## ⚙️ Configuración inicial

1. **Clonar el repositorio**
   ```bash
   git clone https://github.com/Yenny-1301/MovieManagement.git
   cd MovieManagement
2. **Configurar base de datos**
   ```bash 
   "ConectionStrings":{
   "DefaultConnection": "server=localhost;port=3306;database=MovieManagementDb;user=root;password=tu_password"
   }
3. **Aplicar migraciones**
   ```bash
   Add-Migration InitialCreate
   Update-Database
4. **Levantar el Proyecto**
5. **Acceder a Swagger**
   Una vez en ejecución abrir en el navegador
   ```bash 
   https://localhost:5001/swagger

## 👤 Roles Iniciales
Los roles se crean en el `OnModelCreating`.De esta forma quedan disponibles al momento de ejecutar la aplicacion
- Admin
- User

## ✨ Funcionalidades Principales
- CRUD de Peliculas
- Registro y autenticación de usuarios
- Asignacion de roles (Admin, User)
- Documentacion de endpoints con Swagger

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
