using TP2ProgramacionWeb.Frontend.Models;
using TP2ProgramacionWeb.Frontend.Models.ModeloAuxiliar;
using TP2ProgramacionWeb.Frontend.Models.ModeloAuxiliar.Query.Usuario;

namespace TP2ProgramacionWeb.Frontend.Repositories
{
    public interface IUsuarioRepository
    {
        Task<bool> ExistePorUsername(string username);
        Task<bool> ExistePorEmail(string email);
        Task<Usuario?> BuscarPorUsername(string username);
        Task<Usuario?> BuscarPorId(int id);
        Task DarDeBaja(Usuario usuario);
        Task<PaginadoResponse<Usuario>> ObtenerTodos(UsuarioQueryParametros usuarioQueryParametros);
        Task Actualizar(Usuario usuario);
    }
}
