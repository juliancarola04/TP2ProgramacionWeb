namespace TP2ProgramacionWeb.Frontend.Models.ModeloAuxiliar.Query.Cliente;

public record ClienteQueryParametros : QueryParametros
{
    public string? Buscar { get; init; } // Por nombre, DNI o E-Mail
    public string? OrdenarPor { get; init; } = "id"; // "id", "nombre", "email"
    public string? Direccion { get; init; } // "asc", "desc"
}
