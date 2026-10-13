using Lab05.Domain.Entities;
using Lab05.Application.Interfaces;

namespace Lab05.Application.Services
{
    public class EvaluacionService : IEvaluacionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EvaluacionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Evaluacione>> GetAllAsync()
        {
            var repository = _unitOfWork.Repository<Evaluacione>();

            return await repository.GetAllAsync();
        }

        public async Task<Evaluacione> GetByIdAsync(int id)
        {
            var repository = _unitOfWork.Repository<Evaluacione>();

            var evaluacion = await repository.GetByIdAsync(id);

            if (evaluacion == null)
                throw new KeyNotFoundException("La evaluación no existe.");

            return evaluacion;
        }

        public async Task CreateAsync(Evaluacione evaluacion)
        {
            if (evaluacion == null)
                throw new ArgumentException("Datos inválidos.");

            var repository = _unitOfWork.Repository<Evaluacione>();

            await repository.AddAsync(evaluacion);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(Evaluacione evaluacion)
        {
            if (evaluacion == null)
                throw new ArgumentException("Datos inválidos.");

            var repository = _unitOfWork.Repository<Evaluacione>();

            var existente = await repository.GetByIdAsync(evaluacion.IdEvaluacion);

            if (existente == null)
                throw new KeyNotFoundException("La evaluación no existe.");

            repository.Update(evaluacion);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var repository = _unitOfWork.Repository<Evaluacione>();

            var evaluacion = await repository.GetByIdAsync(id);

            if (evaluacion == null)
                throw new KeyNotFoundException("La evaluación no existe.");

            repository.Delete(evaluacion);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
