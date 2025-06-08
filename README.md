# 🏀 Arcade Hoops - Unity + API REST (.NET)

Juego de baloncesto 3D desarrollado en Unity con integración a una API REST construida en .NET Core.

## 🎮 Funcionalidades principales

- Inicio de sesión y registro con token JWT.
- Roles de usuario: jugador y administrador.
- Lanzamiento de balón con trayectoria visual y físicas realistas.
- Registro de partidas y tiros (aciertos/fallos) en base de datos.
- Estadísticas de tiempo, puntos y tiros.
- Panel de resumen de partida.
- Panel de administración (solo para admins).
- Panel de opciones de audio (música y efectos).
- Panel de gestión de cuenta (editar nombre y contraseña).

## 🧪 Usuario de prueba

> Puedes usar este usuario para probar el sistema con permisos de administrador:

- **Email**: `admin@admin.com`  
- **Contraseña**: `admin123`  
- **Rol**: `admin` (configurado manualmente en la base de datos)

## 🛠️ Requisitos

- Unity 2021.3 o superior.
- .NET 6 / ASP.NET Core para ejecutar la API (`API-Baloncesto`).
- Base de datos MySQL (o la usada en tu proyecto).
- Postman (opcional) para probar endpoints.

## 🚀 Cómo ejecutar

1. **API**
   - Ir a la carpeta `API-Baloncesto`.
   - Ejecutar `dotnet run` o abrir en Visual Studio y pulsar "Iniciar".

2. **Unity**
   - Abrir la carpeta del proyecto Unity.
   - Asegúrate de tener configurado el endpoint en scripts como `GameManager.cs`.

3. **Base de datos**
   - Importar el script SQL incluido (si procede).
   - Asegurar que el usuario de prueba `admin@admin.com` existe con rol `admin`.

## 📁 Estructura del proyecto

