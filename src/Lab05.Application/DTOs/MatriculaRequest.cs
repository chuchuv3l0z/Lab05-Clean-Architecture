using System.ComponentModel.DataAnnotations;
using Lab05.Domain.Entities;
namespace Lab05.Application.DTOs;
public class MatriculaRequest
{
    [Required, Range(1, int.MaxValue)]
    public int? IdEstudiante { get; set; }
    [Required, Range(1, int.MaxValue)]
    public int? IdCurso { get; set; }
    [Required, StringLength(20)]
    public string? Semestre { get; set; }
    public Matricula ToEntity() => new()
    {
        IdEstudiante = IdEstudiante,
        IdCurso = IdCurso,
        Semestre = Semestre
    };
}
