using System.ComponentModel.DataAnnotations;
using Lab05.Domain.Entities;
namespace Lab05.Application.DTOs;
public class ProfesoreRequest
{
    [Required, StringLength(100)]
    public string Nombre { get; set; } = string.Empty;
    [StringLength(100)]
    public string? Especialidad { get; set; }
    [EmailAddress, StringLength(100)]
    public string? Correo { get; set; }
    public Profesore ToEntity() => new()
    {
        Nombre = Nombre,
        Especialidad = Especialidad,
        Correo = Correo
    };
}
