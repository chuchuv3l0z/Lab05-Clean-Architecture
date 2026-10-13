using Lab05.Domain.Entities;
using Lab05.Application.Interfaces;

namespace Lab05.Application.Services
{
    public class MateriaService : IMateriaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MateriaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Materia>> GetAllAsync()
        {
            var repository = _unitOfWork.Repository<Materia>();

            return await repository.GetAllAsync();
        }

        public async Task<Materia> GetByIdAsync(int id)
        {
            var repository = _unitOfWork.Repository<Materia>();

            var materia = await repository.GetByIdAsync(id);

            if (materia == null)
                throw new KeyNotFoundException("La materia no existe.");

            return materia;
        }

        public async Task CreateAsync(Materia materia)
        {
            if (materia == null)
                throw new ArgumentException("Datos inválidos.");

            var repository = _unitOfWork.Repository<Materia>();

            await repository.AddAsync(materia);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(Materia materia)
        {
            if (materia == null)
                throw new ArgumentException("Datos inválidos.");

            var repository = _unitOfWork.Repository<Materia>();

            var existente = await repository.GetByIdAsync(materia.IdMateria);

            if (existente == null)
                throw new KeyNotFoundException("La materia no existe.");

            repository.Update(materia);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var repository = _unitOfWork.Repository<Materia>();

            var materia = await repository.GetByIdAsync(id);

            if (materia == null)
                throw new KeyNotFoundException("La materia no existe.");

            repository.Delete(materia);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
