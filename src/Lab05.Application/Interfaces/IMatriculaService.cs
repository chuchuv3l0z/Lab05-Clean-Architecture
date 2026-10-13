using Lab05.Domain.Entities;

namespace Lab05.Application.Interfaces
{
    public interface IMatriculaService
    {
        Task<object> ProcesarMatriculaAsync(
            int idEstudiante,
            int idCurso,
            int idProfesor,
            string semestre);

        Task<IEnumerable<Matricula>> GetAllAsync();
        Task<Matricula> GetByIdAsync(int id);
        Task CreateAsync(Matricula matricula);
        Task UpdateAsync(Matricula matricula);
        Task DeleteAsync(int id);
    }
}
