using Microsoft.EntityFrameworkCore;
using TP2ProgramacionWeb.Frontend.Data;
using TP2ProgramacionWeb.Frontend.Models;
using TP2ProgramacionWeb.Frontend.Repositories;

namespace TP2ProgramacionWeb.Frontend.Implementacion;

public class UsuarioRepositoryPsqlEF : IUsuarioRepository
{
    private readonly DataContext _dataContext;

    public UsuarioRepositoryPsqlEF(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    // IgnoreQueryFilters: los índices únicos también incluyen a los usuarios dados de baja.
    public async Task<bool> ExistePorUsername(string username)
    {
        return await _dataContext.Usuarios.IgnoreQueryFilters().AnyAsync(u => u.Username == username);
    }

    public async Task<bool> ExistePorEmail(string email)
    {
        return await _dataContext.Usuarios.IgnoreQueryFilters().AnyAsync(u => u.Email == email);
    }

    // Solo lectura: AsNoTracking para no dejar la entidad (con su hash) en el contexto del circuito.
    public async Task<Usuario?> BuscarPorUsername(string username)
    {
        return await _dataContext.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task Crear(Usuario usuario)
    {
        try
        {
            _dataContext.Usuarios.Add(usuario);
            await _dataContext.SaveChangesAsync();
        }
        catch
        {
            _dataContext.ChangeTracker.Clear();
            throw;
        }
    }
}