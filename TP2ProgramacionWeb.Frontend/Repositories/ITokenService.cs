using TP2ProgramacionWeb.Frontend.Models;

namespace TP2ProgramacionWeb.Frontend.Repositories
{
    public interface ITokenService
    {
        public (string token, DateTime expiracion) CrearToken(Usuario usuario);
    }
}
