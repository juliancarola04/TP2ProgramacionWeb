namespace TP2ProgramacionWeb.Frontend.DTO.Usuario.Request;

public record ActualizarUsuarioRequest(
    string? Username,
    string? Password,
    string? Email);