namespace TP2ProgramacionWeb.Frontend.DTO.Paginado.Request.Usuario;

public record ParametroPaginacionUsuarioRequest : ParametroPaginacionRequest
{
    public bool? Eliminado { get; init; }
    public bool? EsAdministrador { get; init; }
};