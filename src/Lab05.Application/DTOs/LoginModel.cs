using System.ComponentModel.DataAnnotations;
namespace Lab05.Application.DTOs;
public class LoginModel
{
    [Required, StringLength(100, MinimumLength = 3)]
    public string Username { get; set; } = string.Empty;
    [Required, StringLength(72, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;
}
