using Lab05.Domain.Entities;
using Lab05.Application.Interfaces;

namespace Lab05.Application.Services
{
    public class ProfesoreService : IProfesoreService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProfesoreService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Profesore>> GetAllAsync()
        {
            var repository = _unitOfWork.Repository<Profesore>();

            return await repository.GetAllAsync();
        }

        public async Task<Profesore> GetByIdAsync(int id)
        {
            var repository = _unitOfWork.Repository<Profesore>();

            var profesor = await repository.GetByIdAsync(id);

            if (profesor == null)
                throw new KeyNotFoundException("El profesor no existe.");

            return profesor;
        }

        public async Task CreateAsync(Profesore profesor)
        {
            if (profesor == null)
                throw new ArgumentException("Datos inválidos.");

            var repository = _unitOfWork.Repository<Profesore>();

            await repository.AddAsync(profesor);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(Profesore profesor)
        {
            if (profesor == null)
                throw new ArgumentException("Datos inválidos.");

            var repository = _unitOfWork.Repository<Profesore>();

            var existente = await repository.GetByIdAsync(profesor.IdProfesor);

            if (existente == null)
                throw new KeyNotFoundException("El profesor no existe.");

            repository.Update(profesor);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var repository = _unitOfWork.Repository<Profesore>();

            var profesor = await repository.GetByIdAsync(id);

            if (profesor == null)
                throw new KeyNotFoundException("El profesor no existe.");

            repository.Delete(profesor);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
