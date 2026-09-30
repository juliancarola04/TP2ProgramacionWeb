namespace TP2ProgramacionWeb.Frontend.DTO.Usuario.Response;

public record ObtenerUsuarioResponse(
    int Id,
    string Username,
    string Email,
    bool EsAdministrador);