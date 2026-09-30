namespace TP2ProgramacionWeb.Frontend.Models.ModeloAuxiliar.Query.Usuario;

public record UsuarioQueryParametros : QueryParametros
{
    public bool? Eliminado { get; init; }
    public bool? EsAdministrador { get; init; }
}