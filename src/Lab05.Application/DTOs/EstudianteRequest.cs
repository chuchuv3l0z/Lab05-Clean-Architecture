using System.ComponentModel.DataAnnotations;
using Lab05.Domain.Entities;
namespace Lab05.Application.DTOs;
public class EstudianteRequest
{
    [Required, StringLength(100)]
    public string Nombre { get; set; } = string.Empty;
    [Range(1, 120)]
    public int Edad { get; set; }
    [StringLength(255)]
    public string? Direccion { get; set; }
    [StringLength(20)]
    public string? Telefono { get; set; }
    [EmailAddress, StringLength(100)]
    public string? Correo { get; set; }
    public Estudiante ToEntity() => new()
    {
        Nombre = Nombre,
        Edad = Edad,
        Direccion = Direccion,
        Telefono = Telefono,
        Correo = Correo
    };
}
