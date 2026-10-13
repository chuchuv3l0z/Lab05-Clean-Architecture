using Lab05.Application.DTOs;
using Lab05.Application.Interfaces;
using Lab05.Domain.Entities;
namespace Lab05.Application.Services;
public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUsuarioRepository _usuarios;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher _passwordHasher;
    public AuthService(IUnitOfWork unitOfWork, IUsuarioRepository usuarios,
        ITokenService tokenService, IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _usuarios = usuarios;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
    }
    public async Task RegistrarUsuarioAsync(LoginModel registro, string rol)
    {
        if (rol != "Estudiante" && rol != "Admin")
            throw new ArgumentException("Rol inválido. Usa Estudiante o Admin.");
        ValidarCredenciales(registro);
        if (await _usuarios.FindByUsernameAsync(registro.Username) != null)
            throw new InvalidOperationException("El usuario ya existe.");
        var usuario = new Usuario
        {
            Username = registro.Username,
            PasswordHash = _passwordHasher.Hash(registro.Password),
            Rol = rol
        };
        await _unitOfWork.Repository<Usuario>().AddAsync(usuario);
        await _unitOfWork.SaveChangesAsync();
    }
    public async Task<string> LoginAsync(LoginModel login)
    {
        ValidarCredenciales(login);
        var usuario = await _usuarios.FindByUsernameAsync(login.Username);
        if (usuario == null || !_passwordHasher.Verify(login.Password, usuario.PasswordHash))
            throw new UnauthorizedAccessException("Credenciales incorrectas.");
        return _tokenService.GenerateJwtToken(usuario.Username, usuario.Rol);
    }
    private static void ValidarCredenciales(LoginModel modelo)
    {
        if (string.IsNullOrWhiteSpace(modelo.Username) || modelo.Username.Length is < 3 or > 100)
            throw new ArgumentException("El usuario debe tener entre 3 y 100 caracteres.");
        if (modelo.Password.Length < 6 || System.Text.Encoding.UTF8.GetByteCount(modelo.Password) > 72)
            throw new ArgumentException("La contraseña debe tener al menos 6 caracteres y hasta 72 bytes.");
    }
    public string ObtenerDatosAdmin() => "Información confidencial solo para administradores.";
    public string ObtenerDatosEstudiante() => "Información disponible para estudiantes.";
    public string ObtenerDatosPerfil(string? username) => $"Bienvenido a tu perfil, {username ?? "Usuario"}!";
}
