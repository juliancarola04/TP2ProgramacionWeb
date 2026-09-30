using TP2ProgramacionWeb.Frontend.Models;

namespace TP2ProgramacionWeb.Frontend.Repositories
{
    public interface IRegisterRepository
    {
        Task Registrarse(Usuario usuario);
    }
}
