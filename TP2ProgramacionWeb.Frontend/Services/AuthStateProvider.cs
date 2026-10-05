using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace TP2ProgramacionWeb.Frontend.Services;

public sealed record SesionUsuario(int Id, string Username, string Rol);

public class AuthStateProvider : AuthenticationStateProvider
{
    private const string Clave = "sesion";
    private static readonly AuthenticationState Anonimo = new(new ClaimsPrincipal(new ClaimsIdentity()));

    private readonly ProtectedSessionStorage _almacen;
    private AuthenticationState? _estado;

    public AuthStateProvider(ProtectedSessionStorage almacen)
    {
        _almacen = almacen;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (_estado is not null)
            return _estado;

        try
        {
            var resultado = await _almacen.GetAsync<SesionUsuario>(Clave);
            _estado = resultado.Success && resultado.Value is { } sesion ? Crear(sesion) : Anonimo;
        }
        catch (CryptographicException)
        {
            // Dato corrupto o claves de protección distintas: se descarta la sesión.
            _estado = Anonimo;
        }
        catch (InvalidOperationException)
        {
            // JS interop todavía no disponible: se devuelve anónimo sin cachear.
            return Anonimo;
        }

        return _estado;
    }

    public async Task IniciarSesion(int id, string username, string rol)
    {
        var sesion = new SesionUsuario(id, username, rol);
        await _almacen.SetAsync(Clave, sesion);

        _estado = Crear(sesion);
        NotifyAuthenticationStateChanged(Task.FromResult(_estado));
    }

    public async Task CerrarSesion()
    {
        await _almacen.DeleteAsync(Clave);

        _estado = Anonimo;
        NotifyAuthenticationStateChanged(Task.FromResult(_estado));
    }

    private static AuthenticationState Crear(SesionUsuario sesion)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, sesion.Id.ToString()),
            new Claim(ClaimTypes.Name, sesion.Username),
            new Claim(ClaimTypes.Role, sesion.Rol)
        };

        // El tipo de autenticación no vacío es lo que hace que IsAuthenticated sea true.
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, "SesionBlazor")));
    }
}