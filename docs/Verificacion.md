# Verificación realizada

Fecha: 12 de octubre de 2026.

- Compilación Release de toda la solución: 0 errores y 0 advertencias.
- Verificación automática: 62 comprobaciones correctas.
- Entorno de pruebas: API con TestServer y SQLite temporal en memoria.

Se verificaron registro, inicio de sesión, hash BCrypt, permisos por rol y acceso sin token. En los siete módulos se probaron creación, consulta, actualización y eliminación. También se comprobaron matrícula duplicada, registros inexistentes, procesamiento de matrícula, Swagger y la independencia de Domain y Application.

Para PostgreSQL se verificaron los esquemas del modelo y la traducción de consultas del proveedor Npgsql. No se conectó la API a un servidor PostgreSQL real. Prueba tu conexión y tus tablas desde Swagger antes de entregar.

Repetir la verificación:

```powershell
dotnet run --project tests/Lab05.Verification -c Release
```
