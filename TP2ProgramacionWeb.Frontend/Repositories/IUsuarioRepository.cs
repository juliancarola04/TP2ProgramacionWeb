using TP2ProgramacionWeb.Frontend.Models;

namespace TP2ProgramacionWeb.Frontend.Repositories;

public interface IUsuarioRepository
{
    Task<bool> ExistePorUsername(string username);
    Task<bool> ExistePorEmail(string email);
    Task<Usuario?> BuscarPorUsername(string username);
    Task Crear(Usuario usuario);
}