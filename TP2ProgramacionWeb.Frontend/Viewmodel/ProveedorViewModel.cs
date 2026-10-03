using System.ComponentModel.DataAnnotations;

namespace TP2ProgramacionWeb.Frontend.Viewmodel;

public class ProveedorViewModel
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(30)]
    public string RazonSocial { get; set; }
    
    [Required]
    [StringLength(13)]
    public string CUIT { get; set; }
    
    [Required]
    [StringLength(13)]
    public string Telefono { get; set; }
    
    [Required]
    [StringLength(40)]
    public string Direccion { get; set; }
    
    [Required]
    [StringLength(320)]
    public string Email { get; set; }
}