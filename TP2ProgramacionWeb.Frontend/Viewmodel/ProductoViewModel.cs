using System.ComponentModel.DataAnnotations;

namespace TP2ProgramacionWeb.Frontend.Viewmodel;

public class ProductoViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(60)]
    public string Nombre { get; set; } = "";

    [Range(0, 999999999, ErrorMessage = "El precio de compra no puede ser negativo.")]
    public decimal PrecioCompra { get; set; }

    [Range(0, 999999999, ErrorMessage = "El precio de venta no puede ser negativo.")]
    public decimal PrecioVenta { get; set; }

    public int Stock { get; set; } // solo lectura: lo mueven los ingresos

    [Range(1, int.MaxValue, ErrorMessage = "Seleccioná una categoría.")]
    public int CategoriaId { get; set; }

    public string CategoriaNombre { get; set; } = "";
}