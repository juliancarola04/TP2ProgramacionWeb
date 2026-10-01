using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TP2ProgramacionWeb.Frontend.Options;

namespace TP2ProgramacionWeb.Frontend.Services;

public class JwtAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly TokenValidationParameters _parametros;

    private string? _token;
    private DateTime _expiracion;

    private ClaimsPrincipal _usuario = new(new ClaimsIdentity());

    public JwtAuthenticationStateProvider(IOptions<JwtSettings> options)
    {
        var settings = options.Value;

        _parametros = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(settings.Key)),

            ValidateIssuer = true,
            ValidIssuer = settings.Issuer,

            ValidateAudience = true,
            ValidAudience = settings.Audience,

            ValidateLifetime = true,
            RequireExpirationTime = true,
            RequireSignedTokens = true,
            ClockSkew = TimeSpan.Zero,

            ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 },
            RoleClaimType = ClaimTypes.Role
        };
    }

    public string? ObtenerToken()
    {
        ComprobarExpiracion();
        return _token;
    }
    
    public void ComprobarExpiracion()
    {
        if (_token is not null && DateTime.UtcNow >= _expiracion)
        {
            CerrarSesion();
        }
    }

    public void CerrarSesion()
    {
        _token = null;
        _expiracion = default;
        _usuario = new ClaimsPrincipal(new ClaimsIdentity());

        NotificarCambio();
    }

    private void NotificarCambio()
    {
        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(_usuario)));
    }
    
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        ComprobarExpiracion();

        return Task.FromResult(new AuthenticationState(_usuario));
    }

    public void IniciarSesion(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        var usuario = handler.ValidateToken(
            token,
            _parametros,
            out var tokenValidado);

        _token = token;
        _usuario = usuario;
        _expiracion = tokenValidado.ValidTo;

        NotificarCambio();
    }

}