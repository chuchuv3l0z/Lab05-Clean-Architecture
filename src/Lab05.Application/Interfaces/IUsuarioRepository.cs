using Lab05.Domain.Entities;
namespace Lab05.Application.Interfaces;
public interface IUsuarioRepository
{
    Task<Usuario?> FindByUsernameAsync(string username);
}
