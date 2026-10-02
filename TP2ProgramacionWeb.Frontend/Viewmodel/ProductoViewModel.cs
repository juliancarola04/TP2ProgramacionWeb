using System.ComponentModel.DataAnnotations;

namespace TP2ProgramacionWeb.Frontend.Viewmodel;

public class ProductoViewModel
{
    [Required]
    public string Nombre { get; set; }
    [Required]
    public decimal PrecioCompra { get; set; }
    [Required]
    public decimal PrecioVenta { get; set; }
    [Required]
    public int Stock { get; set; }
}