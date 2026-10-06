using System.ComponentModel.DataAnnotations;

namespace TP2ProgramacionWeb.Frontend.Viewmodel;

public class LoginViewModel
{
    [Required(ErrorMessage = "Ingresá tu usuario.")]
    public string Username { get; set; } = "";

    [Required(ErrorMessage = "Ingresá tu contraseña.")]
    public string Password { get; set; } = "";
}