using System.ComponentModel.DataAnnotations;

namespace TP2ProgramacionWeb.Frontend.Viewmodel;

public class ProveedorViewModel
{
    [Required]
    public string RazonSocial { get; set; }
    
    [Required]
    public string CUIT { get; set; }
    
    [Required]
    public string Telefono { get; set; }
    
    [Required]
    public string Direccion { get; set; }
    
    [Required]
    public string Email { get; set; }
}