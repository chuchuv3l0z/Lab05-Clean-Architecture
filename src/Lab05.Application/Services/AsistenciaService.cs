using Lab05.Domain.Entities;
using Lab05.Application.Interfaces;

namespace Lab05.Application.Services
{
    public class AsistenciaService : IAsistenciaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AsistenciaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Asistencia>> GetAllAsync()
        {
            var repository = _unitOfWork.Repository<Asistencia>();

            return await repository.GetAllAsync();
        }

        public async Task<Asistencia> GetByIdAsync(int id)
        {
            var repository = _unitOfWork.Repository<Asistencia>();

            var asistencia = await repository.GetByIdAsync(id);

            if (asistencia == null)
                throw new KeyNotFoundException("La asistencia no existe.");

            return asistencia;
        }

        public async Task CreateAsync(Asistencia asistencia)
        {
            if (asistencia == null)
                throw new ArgumentException("Datos inválidos.");

            var repository = _unitOfWork.Repository<Asistencia>();

            await repository.AddAsync(asistencia);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(Asistencia asistencia)
        {
            if (asistencia == null)
                throw new ArgumentException("Datos inválidos.");

            var repository = _unitOfWork.Repository<Asistencia>();

            var existente = await repository.GetByIdAsync(asistencia.IdAsistencia);

            if (existente == null)
                throw new KeyNotFoundException("La asistencia no existe.");

            repository.Update(asistencia);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var repository = _unitOfWork.Repository<Asistencia>();

            var asistencia = await repository.GetByIdAsync(id);

            if (asistencia == null)
                throw new KeyNotFoundException("La asistencia no existe.");

            repository.Delete(asistencia);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
