using Lab05.Domain.Entities;

namespace Lab05.Application.Interfaces
{
    public interface IProfesoreService
    {
        Task<IEnumerable<Profesore>> GetAllAsync();
        Task<Profesore> GetByIdAsync(int id);
        Task CreateAsync(Profesore profesor);
        Task UpdateAsync(Profesore profesor);
        Task DeleteAsync(int id);
    }
}
