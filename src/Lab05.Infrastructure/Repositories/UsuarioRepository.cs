using Lab05.Application.Interfaces;
using Lab05.Domain.Entities;
using Lab05.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
namespace Lab05.Infrastructure.Repositories;
public class UsuarioRepository(ApplicationDbContext context) : IUsuarioRepository
{
    public Task<Usuario?> FindByUsernameAsync(string username) =>
        context.Usuarios.AsNoTracking().SingleOrDefaultAsync(u => u.Username == username);
}
