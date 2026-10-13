using Lab05.Application.DTOs;

namespace Lab05.Application.Interfaces
{
    public interface IAuthService
    {
        Task RegistrarUsuarioAsync(LoginModel registro, string rol);
        Task<string> LoginAsync(LoginModel login);
        
        string ObtenerDatosAdmin();
        string ObtenerDatosEstudiante();
        string ObtenerDatosPerfil(string? username);
    }
}
