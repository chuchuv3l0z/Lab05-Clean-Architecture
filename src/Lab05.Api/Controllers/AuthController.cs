using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; 
using Lab05.Application.Interfaces;
using System;
using Lab05.Application.DTOs;

namespace Lab05.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] LoginModel modelo, [FromQuery] string rol = "Estudiante")
        {
            try
            {
                // Solo un administrador autenticado puede crear otro administrador.
                if (rol == "Admin" && !User.IsInRole("Admin"))
                    return StatusCode(StatusCodes.Status403Forbidden,
                        new { message = "Necesitas ser administrador para asignar ese rol." });
                await _authService.RegistrarUsuarioAsync(modelo, rol);
                return Ok(new { message = "Usuario registrado exitosamente." });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            try
            {
                var token = await _authService.LoginAsync(login);
                return Ok(new { token });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin-data")]
        public IActionResult GetAdminData() => Ok(new { message = _authService.ObtenerDatosAdmin() });

        [Authorize(Roles = "Estudiante")]
        [HttpGet("estudiante-data")]
        public IActionResult GetEstudianteData() => Ok(new { message = _authService.ObtenerDatosEstudiante() });

        [Authorize]
        [HttpGet("perfil")]
        public IActionResult GetPerfil() => Ok(new { message = _authService.ObtenerDatosPerfil(User.Identity?.Name) });
    }
}
