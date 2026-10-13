using System;
using System.Collections.Generic;

namespace Lab05.Domain.Entities;

public partial class Matricula
{
    public void Validar()
    {
        if (IdEstudiante is null or <= 0 || IdCurso is null or <= 0)
            throw new ArgumentException("La matrícula necesita un estudiante y un curso.");
        if (string.IsNullOrWhiteSpace(Semestre) || Semestre.Length > 20)
            throw new ArgumentException("El semestre es obligatorio y debe tener hasta 20 caracteres.");
    }

    public int IdMatricula { get; set; }

    public int? IdEstudiante { get; set; }

    public int? IdCurso { get; set; }

    public string? Semestre { get; set; }

    public virtual Curso? IdCursoNavigation { get; set; }

    public virtual Estudiante? IdEstudianteNavigation { get; set; }
}
