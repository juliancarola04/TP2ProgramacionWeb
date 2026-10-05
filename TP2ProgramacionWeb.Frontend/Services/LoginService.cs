using TP2ProgramacionWeb.Frontend.DTO.Auth.Request;
using TP2ProgramacionWeb.Frontend.DTO.Auth.Response;
using TP2ProgramacionWeb.Frontend.Excepciones;
using TP2ProgramacionWeb.Frontend.Models;
using TP2ProgramacionWeb.Frontend.Repositories;
using TP2ProgramacionWeb.Frontend.Utilidades;

namespace TP2ProgramacionWeb.Frontend.Services;

public class LoginService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public LoginService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<LoginResponse> Login(LoginRequest request)
    {
        string username = request.Username?.Trim() ?? "";
        string password = request.Password ?? "";

        if (!Validaciones.EstanDatosBien(username, password))
            throw new DatosLlegaronErradosException("Ingresá el usuario y la contraseña.");

        Usuario? usuario = await _usuarioRepository.BuscarPorUsername(username);

        // Mismo mensaje para "no existe" y "contraseña incorrecta": no revela qué usuarios existen.
        if (usuario is null || !BCrypt.Net.BCrypt.EnhancedVerify(password, usuario.Password))
            throw new DatosLlegaronErradosException("Usuario o contraseña incorrectos.");

        return new LoginResponse(usuario.Id, usuario.Username, Roles.De(usuario));
    }
}
