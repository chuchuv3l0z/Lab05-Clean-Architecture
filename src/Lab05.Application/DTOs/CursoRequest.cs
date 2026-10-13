using System.ComponentModel.DataAnnotations;
using Lab05.Domain.Entities;
namespace Lab05.Application.DTOs;
public class CursoRequest
{
    [Required, StringLength(100)]
    public string Nombre { get; set; } = string.Empty;
    
    public string? Descripcion { get; set; }
    [Range(1, 30)]
    public int Creditos { get; set; }
    public Curso ToEntity() => new()
    {
        Nombre = Nombre,
        Descripcion = Descripcion,
        Creditos = Creditos
    };
}
