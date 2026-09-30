namespace TP2ProgramacionWeb.Frontend.DTO.Auth.Request;

public record RegisterRequest(
    string Username,
    string Password,
    string Email);