using Lab05.Domain.Entities;
using Lab05.Application.Interfaces;

namespace Lab05.Application.Services
{
    public class EstudianteService : IEstudianteService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EstudianteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Estudiante>> GetAllAsync()
        {
            var repository = _unitOfWork.Repository<Estudiante>();

            return await repository.GetAllAsync();
        }

        public async Task<Estudiante> GetByIdAsync(int id)
        {
            var repository = _unitOfWork.Repository<Estudiante>();

            var estudiante = await repository.GetByIdAsync(id);

            if (estudiante == null)
                throw new KeyNotFoundException("El estudiante no existe.");

            return estudiante;
        }

        public async Task CreateAsync(Estudiante estudiante)
        {
            if (estudiante == null)
                throw new ArgumentException("Datos inválidos.");

            var repository = _unitOfWork.Repository<Estudiante>();

            await repository.AddAsync(estudiante);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(Estudiante estudiante)
        {
            if (estudiante == null)
                throw new ArgumentException("Datos inválidos.");

            var repository = _unitOfWork.Repository<Estudiante>();

            var existente = await repository.GetByIdAsync(estudiante.IdEstudiante);

            if (existente == null)
                throw new KeyNotFoundException("El estudiante no existe.");

            repository.Update(estudiante);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var repository = _unitOfWork.Repository<Estudiante>();

            var estudiante = await repository.GetByIdAsync(id);

            if (estudiante == null)
                throw new KeyNotFoundException("El estudiante no existe.");

            repository.Delete(estudiante);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
