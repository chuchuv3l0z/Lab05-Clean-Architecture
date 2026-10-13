using Lab05.Domain.Entities;

namespace Lab05.Application.Interfaces
{
    public interface ICursoService
    {
        Task<IEnumerable<Curso>> GetAllAsync();
        Task<Curso> GetByIdAsync(int id);
        Task CreateAsync(Curso curso);
        Task UpdateAsync(Curso curso);
        Task DeleteAsync(int id);
    }
}
