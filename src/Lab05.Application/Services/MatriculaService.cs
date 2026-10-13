using Lab05.Domain.Entities;
using Lab05.Application.Interfaces;

namespace Lab05.Application.Services
{
    public class MatriculaService : IMatriculaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEstudianteService _estudianteService;
        private readonly ICursoService _cursoService;
        private readonly IProfesoreService _profesorService;

        public MatriculaService(
            IUnitOfWork unitOfWork,
            IEstudianteService estudianteService,
            ICursoService cursoService,
            IProfesoreService profesorService)
        {
            _unitOfWork = unitOfWork;
            _estudianteService = estudianteService;
            _cursoService = cursoService;
            _profesorService = profesorService;
        }

        public async Task<object> ProcesarMatriculaAsync(
            int idEstudiante,
            int idCurso,
            int idProfesor,
            string semestre)
        {
            var docente = await _profesorService.GetByIdAsync(idProfesor);

            var estudiante = await _estudianteService.GetByIdAsync(idEstudiante);

            var curso = await _cursoService.GetByIdAsync(idCurso);

            var nuevaMatricula = new Matricula
            {
                IdEstudiante = idEstudiante,
                IdCurso = idCurso,
                Semestre = semestre
            };

            await CreateAsync(nuevaMatricula);

            return new
            {
                Mensaje = "Lógica de matrícula ejecutada con éxito",
                Detalle = $"El estudiante {estudiante.Nombre} ha sido matriculado en el curso {curso.Nombre} a cargo del docente {docente.Nombre} para el semestre {semestre}."
            };
        }

        private async Task ValidarMatriculaAsync(Matricula matricula)
        {
            matricula.Validar();
            await _estudianteService.GetByIdAsync(matricula.IdEstudiante!.Value);
            await _cursoService.GetByIdAsync(matricula.IdCurso!.Value);
            var duplicada = await _unitOfWork.Repository<Matricula>().AnyAsync(m =>
                m.IdEstudiante == matricula.IdEstudiante && m.IdCurso == matricula.IdCurso &&
                m.Semestre == matricula.Semestre && m.IdMatricula != matricula.IdMatricula);
            if (duplicada)
                throw new InvalidOperationException("El estudiante ya está matriculado en ese curso y semestre.");
        }

        public async Task<IEnumerable<Matricula>> GetAllAsync()
        {
            var repository = _unitOfWork.Repository<Matricula>();

            return await repository.GetAllAsync();
        }

        public async Task<Matricula> GetByIdAsync(int id)
        {
            var repository = _unitOfWork.Repository<Matricula>();

            var matricula = await repository.GetByIdAsync(id);

            if (matricula == null)
                throw new KeyNotFoundException("La matrícula no existe.");

            return matricula;
        }

        public async Task CreateAsync(Matricula matricula)
        {
            if (matricula == null)
                throw new ArgumentException("Datos inválidos.");

            await ValidarMatriculaAsync(matricula);

            var repository = _unitOfWork.Repository<Matricula>();

            await repository.AddAsync(matricula);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(Matricula matricula)
        {
            if (matricula == null)
                throw new ArgumentException("Datos inválidos.");

            await ValidarMatriculaAsync(matricula);

            var repository = _unitOfWork.Repository<Matricula>();

            var existente = await repository.GetByIdAsync(matricula.IdMatricula);

            if (existente == null)
                throw new KeyNotFoundException("La matrícula no existe.");

            repository.Update(matricula);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var repository = _unitOfWork.Repository<Matricula>();

            var matricula = await repository.GetByIdAsync(id);

            if (matricula == null)
                throw new KeyNotFoundException("La matrícula no existe.");

            repository.Delete(matricula);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
