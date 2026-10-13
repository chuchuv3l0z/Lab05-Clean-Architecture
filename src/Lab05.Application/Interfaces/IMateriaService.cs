using Lab05.Domain.Entities;

namespace Lab05.Application.Interfaces
{
    public interface IMateriaService
    {
        Task<IEnumerable<Materia>> GetAllAsync();
        Task<Materia> GetByIdAsync(int id);
        Task CreateAsync(Materia materia);
        Task UpdateAsync(Materia materia);
        Task DeleteAsync(int id);
    }
}
