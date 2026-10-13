using Lab05.Domain.Entities;
using Lab05.Application.Interfaces;

namespace Lab05.Application.Services
{
    public class CursoService : ICursoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CursoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Curso>> GetAllAsync()
        {
            var repository = _unitOfWork.Repository<Curso>();

            return await repository.GetAllAsync();
        }

        public async Task<Curso> GetByIdAsync(int id)
        {
            var repository = _unitOfWork.Repository<Curso>();

            var curso = await repository.GetByIdAsync(id);

            if (curso == null)
                throw new KeyNotFoundException("El curso no existe.");

            return curso;
        }

        public async Task CreateAsync(Curso curso)
        {
            if (curso == null)
                throw new ArgumentException("Datos inválidos.");

            var repository = _unitOfWork.Repository<Curso>();

            await repository.AddAsync(curso);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(Curso curso)
        {
            if (curso == null)
                throw new ArgumentException("Datos inválidos.");

            var repository = _unitOfWork.Repository<Curso>();

            var existente = await repository.GetByIdAsync(curso.IdCurso);

            if (existente == null)
                throw new KeyNotFoundException("El curso no existe.");

            repository.Update(curso);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var repository = _unitOfWork.Repository<Curso>();

            var curso = await repository.GetByIdAsync(id);

            if (curso == null)
                throw new KeyNotFoundException("El curso no existe.");

            repository.Delete(curso);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
