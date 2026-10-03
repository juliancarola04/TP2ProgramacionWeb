using System.ComponentModel.DataAnnotations;

namespace TP2ProgramacionWeb.Frontend.Viewmodel;

public class CategoriaViewModel
{
    public int Id { get; set; }
    [Required] 
    [StringLength(30)]
    public string Nombre { get; set; }
    
    [Required] 
    [StringLength(600)]
    public string Descripcion { get; set; }
}