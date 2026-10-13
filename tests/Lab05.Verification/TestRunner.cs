using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Lab05.Domain.Entities;
using Lab05.Infrastructure.Persistence;
using Lab05.Infrastructure.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

// La API usa PostgreSQL. Esta prueba usa SQLite temporal y TestServer en memoria.
// Así se comprueban HTTP, JWT, servicios y seguimiento de EF sin una BD externa.
internal class TestRunner
{
    private static int _checks;
    public static async Task Main()
    {
        Environment.SetEnvironmentVariable("JWT__Key", "ClaveTemporalSoloParaVerificacion1234567890");
        Environment.SetEnvironmentVariable("ConnectionStrings__DefaultConnection",
            "Host=localhost;Database=verificacion;Username=prueba;Password=prueba");
        using var factory = new VerificationFactory();
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        await Status(await client.GetAsync("/api/Estudiantes"), 401, "Acceso sin token");
        await Status(await client.PostAsJsonAsync("/api/Auth/registrar?rol=Admin",
            new { username = "admin_prueba", password = "Clave12345" }), 403, "Registro público de administrador bloqueado");
        await Status(await client.PostAsJsonAsync("/api/Auth/registrar",
            new { username = "usuario_prueba", password = "Clave12345" }), 200, "Registro de estudiante");
        await Status(await client.PostAsJsonAsync("/api/Auth/registrar",
            new { username = "usuario_prueba", password = "Clave12345" }), 409, "Usuario duplicado");
        await Status(await client.PostAsJsonAsync("/api/Auth/login",
            new { username = "usuario_prueba", password = "Incorrecta123" }), 401, "Contraseña incorrecta");
        var login = await client.PostAsJsonAsync("/api/Auth/login",
            new { username = "usuario_prueba", password = "Clave12345" });
        await Status(login, 200, "Inicio de sesión");
        var token = (await login.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("token").GetString();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        await Status(await client.GetAsync("/api/Auth/perfil"), 200, "Perfil con token");
        await Status(await client.GetAsync("/api/Auth/admin-data"), 403, "Estudiante sin permiso de administrador");
        await Status(await client.GetAsync("/api/Auth/estudiante-data"), 200, "Rol Estudiante");
        await Status(await client.GetAsync("/api/Estudiantes/999999"), 404, "Registro inexistente");
        await Status(await client.PostAsJsonAsync("/api/Estudiantes", new { nombre = "", edad = 20 }), 400, "Validación de nombre");

        var estudiante = await CrudCreateAndUpdate(client, "Estudiantes", "idEstudiante",
            new { nombre = "Jorge Prueba", edad = 20, correo = "prueba@example.com" },
            new { nombre = "Jorge Actualizado", edad = 21, correo = "prueba@example.com" }, "nombre", "Jorge Actualizado");
        var curso = await CrudCreateAndUpdate(client, "Cursos", "idCurso",
            new { nombre = "Arquitectura", creditos = 3 }, new { nombre = "Arquitectura II", creditos = 4 }, "nombre", "Arquitectura II");
        var profesor = await CrudCreateAndUpdate(client, "Profesores", "idProfesor",
            new { nombre = "Docente Prueba", especialidad = "Software" }, new { nombre = "Docente Actualizado", especialidad = "Software" }, "nombre", "Docente Actualizado");
        var materia = await CrudCreateAndUpdate(client, "Materias", "idMateria",
            new { nombre = "Clean Architecture", idCurso = curso }, new { nombre = "Dependencias", idCurso = curso }, "nombre", "Dependencias");
        var asistencia = await CrudCreateAndUpdate(client, "Asistencias", "idAsistencia",
            new { idEstudiante = estudiante, idCurso = curso, fecha = "2026-10-12", estado = "Presente" },
            new { idEstudiante = estudiante, idCurso = curso, fecha = "2026-10-12", estado = "Tardanza" }, "estado", "Tardanza");
        var evaluacion = await CrudCreateAndUpdate(client, "Evaluaciones", "idEvaluacion",
            new { idEstudiante = estudiante, idCurso = curso, fecha = "2026-10-12", calificacion = 18 },
            new { idEstudiante = estudiante, idCurso = curso, fecha = "2026-10-12", calificacion = 19 }, "calificacion", "19");
        var matricula = await CrudCreateAndUpdate(client, "Matriculas", "idMatricula",
            new { idEstudiante = estudiante, idCurso = curso, semestre = "2026-I" },
            new { idEstudiante = estudiante, idCurso = curso, semestre = "2026-II" }, "semestre", "2026-II");
        await Status(await client.PostAsJsonAsync("/api/Matriculas",
            new { idEstudiante = estudiante, idCurso = curso, semestre = "2026-II" }), 409, "Matrícula duplicada");
        await Status(await client.PostAsJsonAsync("/api/Matriculas",
            new { idEstudiante = 999999, idCurso = curso, semestre = "2026-II" }), 404, "Estudiante de matrícula inexistente");
        await Status(await client.PostAsync($"/api/Matriculas/ProcesarMatricula?idEstudiante={estudiante}&idCurso={curso}&idProfesor={profesor}&semestre=2027-I", null),
            200, "Caso de uso ProcesarMatricula");
        await Status(await client.PostAsync($"/api/Matriculas/ProcesarMatricula?idEstudiante={estudiante}&idCurso={curso}&idProfesor=999999&semestre=2027-II", null),
            404, "Profesor inexistente sin guardar matrícula");
        foreach (var module in new[] { "Matriculas", "Asistencias", "Evaluaciones", "Materias" })
        {
            var rows = await client.GetFromJsonAsync<JsonElement[]>($"/api/{module}");
            var key = module switch { "Matriculas" => "idMatricula", "Asistencias" => "idAsistencia", "Evaluaciones" => "idEvaluacion", _ => "idMateria" };
            foreach (var row in rows!)
                await Status(await client.DeleteAsync($"/api/{module}/{row.GetProperty(key).GetInt32()}"), 204, $"Eliminar {module}");
        }
        foreach (var item in new[] { ("Estudiantes", estudiante), ("Cursos", curso), ("Profesores", profesor) })
        {
            await Status(await client.DeleteAsync($"/api/{item.Item1}/{item.Item2}"), 204, $"Eliminar {item.Item1}");
            await Status(await client.GetAsync($"/api/{item.Item1}/{item.Item2}"), 404, $"Confirmar eliminación {item.Item1}");
        }
        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var user = await db.Usuarios.SingleAsync();
            Check(user.PasswordHash != "Clave12345" && new PasswordHasher().Verify("Clave12345", user.PasswordHash), "Contraseña guardada como hash BCrypt");
            user.Rol = "Admin";
            await db.SaveChangesAsync();
        }
        login = await client.PostAsJsonAsync("/api/Auth/login", new { username = "usuario_prueba", password = "Clave12345" });
        token = (await login.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("token").GetString();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        await Status(await client.GetAsync("/api/Auth/admin-data"), 200, "Rol Admin");
        await Status(await client.PostAsJsonAsync("/api/Auth/registrar?rol=Admin",
            new { username = "segundo_admin", password = "Clave12345" }), 200, "Admin puede crear otro Admin");
        using var pg = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql("Host=localhost;Database=verificacion;Username=prueba;Password=prueba").Options);
        var sql = pg.Matriculas.Where(m => m.IdEstudiante == 1).ToQueryString();
        Check(sql.Contains("lab05.matriculas"), "Traducción de consultas del proveedor PostgreSQL");
        var ddl = pg.Database.GenerateCreateScript();
        Check(ddl.Contains("public.usuarios") && ddl.Contains("lab05.estudiantes"), "Mapeo PostgreSQL de tablas y esquemas");
        foreach (var assembly in new[] { typeof(Matricula).Assembly, typeof(Lab05.Application.Services.MatriculaService).Assembly })
            Check(!assembly.GetReferencedAssemblies().Any(a => a.Name!.Contains("EntityFrameworkCore") || a.Name.Contains("Npgsql") || a.Name.Contains("AspNetCore")), $"Independencia de {assembly.GetName().Name}");
        await Status(await client.GetAsync("/swagger/v1/swagger.json"), 200, "Documento Swagger");
        Console.WriteLine($"RESULTADO: {_checks} comprobaciones correctas.");
    }

    private static async Task<int> CrudCreateAndUpdate(HttpClient client, string module, string key,
        object create, object update, string property, string expected)
    {
        var response = await client.PostAsJsonAsync($"/api/{module}", create);
        await Status(response, 201, $"Crear {module}");
        var id = (await response.Content.ReadFromJsonAsync<JsonElement>()).GetProperty(key).GetInt32();
        await Status(await client.PutAsJsonAsync($"/api/{module}/{id}", update), 204, $"Actualizar {module} sin conflicto de seguimiento");
        var detail = await client.GetFromJsonAsync<JsonElement>($"/api/{module}/{id}");
        var value = detail.GetProperty(property);
        var matches = value.ValueKind == JsonValueKind.Number
            ? value.GetDecimal() == decimal.Parse(expected, System.Globalization.CultureInfo.InvariantCulture)
            : value.GetString() == expected;
        Check(matches, $"Datos actualizados de {module}");
        await Status(await client.GetAsync($"/api/{module}"), 200, $"Listar {module}");
        return id;
    }
    private static async Task Status(HttpResponseMessage response, int expected, string name)
    {
        if ((int)response.StatusCode != expected)
            throw new Exception($"{name}: esperaba {expected}, recibió {(int)response.StatusCode}. {await response.Content.ReadAsStringAsync()}");
        Check(true, name);
    }
    private static void Check(bool ok, string name)
    {
        if (!ok) throw new Exception(name);
        _checks++;
        Console.WriteLine($"OK: {name}");
    }
}

internal sealed class VerificationFactory : WebApplicationFactory<Program>
{
    private readonly SqliteConnection _connection = new("Data Source=:memory:");
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _connection.Open();
        var root = new DirectoryInfo(AppContext.BaseDirectory);
        while (root != null && !File.Exists(Path.Combine(root.FullName, "Lab05-JorgeVilca.sln")))
            root = root.Parent;
        if (root == null) throw new DirectoryNotFoundException("No se encontró la solución del laboratorio.");
        builder.UseContentRoot(Path.Combine(root.FullName, "src", "Lab05.Api"));
        builder.UseEnvironment("Development");
        builder.ConfigureAppConfiguration((_, config) => config.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["JWT:Issuer"] = "Lab05.Api", ["JWT:Audience"] = "Lab05.Client",
            ["Logging:LogLevel:Default"] = "Warning"
        }));
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<ApplicationDbContext>();
            services.RemoveAll<DbContextOptions<ApplicationDbContext>>();
            services.RemoveAll<Microsoft.EntityFrameworkCore.Infrastructure.IDbContextOptionsConfiguration<ApplicationDbContext>>();
            services.AddScoped<ApplicationDbContext>(_ => new SqliteVerificationContext(
                new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(_connection).Options));
            using var provider = services.BuildServiceProvider();
            using var scope = provider.CreateScope();
            scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.EnsureCreated();
        });
    }
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing) _connection.Dispose();
    }
}

internal sealed class SqliteVerificationContext(DbContextOptions<ApplicationDbContext> options) : ApplicationDbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            entity.SetSchema(null);
            foreach (var property in entity.FindPrimaryKey()!.Properties)
            {
                property.SetDefaultValueSql(null);
                property.ValueGenerated = ValueGenerated.OnAdd;
            }
        }
    }
}
