using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
namespace Lab05.Api.Middleware;
public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try { await next(context); }
        catch (Exception ex)
        {
            var (status, message) = ex switch
            {
                KeyNotFoundException => (404, ex.Message),
                ArgumentException => (400, ex.Message),
                InvalidOperationException => (409, ex.Message),
                UnauthorizedAccessException => (401, ex.Message),
                DbUpdateException { InnerException: PostgresException { SqlState: "23503" } }
                    => (409, "La operación afecta relaciones con otros registros. Revisa los identificadores."),
                DbUpdateException { InnerException: PostgresException { SqlState: "23505" } }
                    => (409, "Ya existe un registro con esos datos únicos."),
                _ => (500, "No se pudo completar la operación. Revisa la configuración y los registros de la API.")
            };
            if (status == 500) logger.LogError(ex, "Error al procesar la solicitud");
            context.Response.StatusCode = status;
            await context.Response.WriteAsJsonAsync(new ProblemDetails
            { Status = status, Title = message, Instance = context.Request.Path });
        }
    }
}
