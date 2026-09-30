namespace TP2ProgramacionWeb.Frontend.DTO.Paginado.Request.Categoria;

public record ParametroPaginacionCategoriaRequest : ParametroPaginacionRequest
{
    public string? Buscar { get; init; } // Por nombre
    public string? OrdenarPor { get; init; } // "id", "nombre"
    public string? Direccion { get; init; } // "asc", "desc"
}
