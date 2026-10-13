using Lab05.Domain.Entities;

namespace Lab05.Application.Interfaces
{
    public interface IAsistenciaService
    {
        Task<IEnumerable<Asistencia>> GetAllAsync();
        Task<Asistencia> GetByIdAsync(int id);
        Task CreateAsync(Asistencia asistencia);
        Task UpdateAsync(Asistencia asistencia);
        Task DeleteAsync(int id);
    }
}
