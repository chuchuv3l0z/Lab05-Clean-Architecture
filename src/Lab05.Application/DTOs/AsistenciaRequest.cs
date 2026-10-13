using System.ComponentModel.DataAnnotations;
using Lab05.Domain.Entities;
namespace Lab05.Application.DTOs;
public class AsistenciaRequest
{
    [Required, Range(1, int.MaxValue)]
    public int? IdEstudiante { get; set; }
    [Required, Range(1, int.MaxValue)]
    public int? IdCurso { get; set; }
    
    public DateOnly? Fecha { get; set; }
    [StringLength(20)]
    public string? Estado { get; set; }
    public Asistencia ToEntity() => new()
    {
        IdEstudiante = IdEstudiante,
        IdCurso = IdCurso,
        Fecha = Fecha,
        Estado = Estado
    };
}
