using System.ComponentModel.DataAnnotations;
using Lab05.Domain.Entities;
namespace Lab05.Application.DTOs;
public class MateriaRequest
{
    [Required, Range(1, int.MaxValue)]
    public int? IdCurso { get; set; }
    [Required, StringLength(100)]
    public string Nombre { get; set; } = string.Empty;
    
    public string? Descripcion { get; set; }
    public Materia ToEntity() => new()
    {
        IdCurso = IdCurso,
        Nombre = Nombre,
        Descripcion = Descripcion
    };
}
