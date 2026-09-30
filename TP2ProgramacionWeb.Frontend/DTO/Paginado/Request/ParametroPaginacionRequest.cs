namespace TP2ProgramacionWeb.Frontend.DTO.Paginado.Request;

public record ParametroPaginacionRequest
{
    public int? NumeroPagina { get; init; }
    public int? TamanoPagina { get; init; }
}
    