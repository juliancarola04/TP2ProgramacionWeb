using System.ComponentModel.DataAnnotations;

namespace TP2ProgramacionWeb.Frontend.Viewmodel;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Ingresá un usuario.")]
    public string Username { get; set; } = "";
    
    [Required(ErrorMessage = "Ingresá una contraseña.")]
    [MinLength(12, ErrorMessage = "La contraseña debe de tener como mínimo 12 caracteres.")]
    public string Password { get; set; } = "";
    
    [Required(ErrorMessage = "Ingresá un E-Mail")]
    [EmailAddress(ErrorMessage = "El formato del E-Mail es incorrecto.")]
    public string Email { get; set; } = "";
}