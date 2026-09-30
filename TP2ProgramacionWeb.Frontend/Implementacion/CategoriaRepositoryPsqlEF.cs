using Microsoft.EntityFrameworkCore;
using TP2ProgramacionWeb.Frontend.Data;
using TP2ProgramacionWeb.Frontend.Models;
using TP2ProgramacionWeb.Frontend.Models.ModeloAuxiliar;
using TP2ProgramacionWeb.Frontend.Models.ModeloAuxiliar.Query.Categoria;
using TP2ProgramacionWeb.Frontend.Repositories;

namespace TP2ProgramacionWeb.Frontend.Implementacion
{
    public class CategoriaRepositoryPsqlEF : ICategoriaRepository
    {
        private readonly DataContext _dataContext;
        public CategoriaRepositoryPsqlEF (DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<PaginadoResponse<Categoria>> ObtenerTodas(CategoriaQueryParametros parametros)
        {
            IQueryable<Categoria> query = _dataContext.Categorias.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(parametros.Buscar))
            {
                string buscar = parametros.Buscar.ToLower();
                query = query.Where(c => c.Nombre.ToLower().Contains(buscar));
            }

            bool descendente = parametros.Direccion?.Equals("desc", StringComparison.OrdinalIgnoreCase) == true;
            query = parametros.OrdenarPor?.ToLower() switch
            {
                "nombre" => descendente ? query.OrderByDescending(c => c.Nombre).ThenBy(c => c.Id) : query.OrderBy(c => c.Nombre).ThenBy(c => c.Id),
                _ => descendente ? query.OrderByDescending(c => c.Id) : query.OrderBy(c => c.Id)
            };

            int totalRegistros = await query.CountAsync();
            List<Categoria> categorias = await query
                .Skip((parametros.NumeroPagina - 1) * parametros.TamanoPagina)
                .Take(parametros.TamanoPagina)
                .ToListAsync();

            return new PaginadoResponse<Categoria>(categorias, parametros.NumeroPagina, parametros.TamanoPagina, totalRegistros);
        }
        public async Task<Categoria?> ObtenerPorId(int id)
        {
            return await _dataContext.Categorias
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<bool> ExistePorNombre(string nombre)
        {
            return await _dataContext.Categorias.AnyAsync(c => c.Nombre == nombre);
        }
        public async Task<bool> TieneProductosAsociados(int categoriaId)
        {
            return await _dataContext.Productos.AnyAsync(p => p.CategoriaId == categoriaId);
        }
        public async Task Crear(Categoria categoria)
        {
            _dataContext.Categorias.Add(categoria);
            await _dataContext.SaveChangesAsync();
        }
        public async Task Actualizar(Categoria categoria)
        {
            _dataContext.Categorias.Update(categoria);
            await _dataContext.SaveChangesAsync();
        }
        public async Task Eliminar(Categoria categoria)
        {
            _dataContext.Categorias.Remove(categoria);
            await _dataContext.SaveChangesAsync();
        }

    }
}
