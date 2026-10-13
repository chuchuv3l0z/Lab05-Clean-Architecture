using Lab05.Domain.Entities;

namespace Lab05.Application.Interfaces
{
    public interface IEvaluacionService
    {
        Task<IEnumerable<Evaluacione>> GetAllAsync();
        Task<Evaluacione> GetByIdAsync(int id);
        Task CreateAsync(Evaluacione evaluacion);
        Task UpdateAsync(Evaluacione evaluacion);
        Task DeleteAsync(int id);
    }
}
