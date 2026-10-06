using System.ComponentModel.DataAnnotations;

namespace TP2ProgramacionWeb.Frontend.Viewmodel;

public class RegisterViewModel
{
    [Required(ErrorMessage = "El usuario es obligatorio.")]
    [StringLength(30, ErrorMessage = "El usuario no puede superar los 30 caracteres.")]
    public string Username { get; set; } = "";

    [Required(ErrorMessage = "El E-Mail es obligatorio.")]
    [EmailAddress(ErrorMessage = "El formato del E-Mail es inválido.")]
    [StringLength(320)]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
    public string Password { get; set; } = "";

    [Compare(nameof(Password), ErrorMessage = "Las contraseñas no coinciden.")]
    public string ConfirmarPassword { get; set; } = "";
}