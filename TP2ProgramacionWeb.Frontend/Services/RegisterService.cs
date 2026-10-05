using System.ComponentModel.DataAnnotations;
using TP2ProgramacionWeb.Frontend.DTO.Auth.Request;
using TP2ProgramacionWeb.Frontend.DTO.Auth.Response;
using TP2ProgramacionWeb.Frontend.Excepciones;
using TP2ProgramacionWeb.Frontend.Models;
using TP2ProgramacionWeb.Frontend.Repositories;
using TP2ProgramacionWeb.Frontend.Utilidades;

namespace TP2ProgramacionWeb.Frontend.Services;

public class RegisterService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public RegisterService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<RegisterResponse> Registrarse(RegisterRequest request)
    {
        string username = request.Username?.Trim() ?? "";
        string email = request.Email?.Trim().ToLowerInvariant() ?? "";
        string password = request.Password ?? "";

        if (!Validaciones.EstanDatosBien(username, password, email))
            throw new DatosLlegaronErradosException("Alguno de los datos llegó vacío.");

        if (username.Length > 30)
            throw new DatosLlegaronErradosException("El usuario no puede superar los 30 caracteres.");

        if (!new EmailAddressAttribute().IsValid(email))
            throw new DatosLlegaronErradosException("El formato del E-Mail es inválido.");

        if (password.Length < 6)
            throw new DatosLlegaronErradosException("La contraseña debe tener al menos 6 caracteres.");

        if (await _usuarioRepository.ExistePorUsername(username))
            throw new RecursoExistenteException("Ya existe alguien con ese usuario.");

        if (await _usuarioRepository.ExistePorEmail(email))
            throw new RecursoExistenteException("Ya existe alguien con ese email.");

        Usuario usuario = new Usuario
        {
            Username = username,
            Password = BCrypt.Net.BCrypt.EnhancedHashPassword(password),
            Email = email
        };

        await _usuarioRepository.Crear(usuario);

        return new RegisterResponse(usuario.Id, usuario.Username, Roles.De(usuario));
    }
}