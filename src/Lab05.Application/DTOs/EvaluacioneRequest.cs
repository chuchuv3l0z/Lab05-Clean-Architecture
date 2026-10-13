using System.ComponentModel.DataAnnotations;
using Lab05.Domain.Entities;
namespace Lab05.Application.DTOs;
public class EvaluacioneRequest
{
    [Required, Range(1, int.MaxValue)]
    public int? IdEstudiante { get; set; }
    [Required, Range(1, int.MaxValue)]
    public int? IdCurso { get; set; }
    [Range(0, 20)]
    public decimal? Calificacion { get; set; }
    
    public DateOnly? Fecha { get; set; }
    public Evaluacione ToEntity() => new()
    {
        IdEstudiante = IdEstudiante,
        IdCurso = IdCurso,
        Calificacion = Calificacion,
        Fecha = Fecha
    };
}
