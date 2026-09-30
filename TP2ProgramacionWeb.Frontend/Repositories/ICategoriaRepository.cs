using TP2ProgramacionWeb.Frontend.Models;
using TP2ProgramacionWeb.Frontend.Models.ModeloAuxiliar;
using TP2ProgramacionWeb.Frontend.Models.ModeloAuxiliar.Query.Categoria;

namespace TP2ProgramacionWeb.Frontend.Repositories
{
    public interface ICategoriaRepository
    {
        Task<PaginadoResponse<Categoria>> ObtenerTodas(CategoriaQueryParametros parametros);
        Task<Categoria?> ObtenerPorId(int id);
        Task<bool> ExistePorNombre(string nombre);
        Task<bool> TieneProductosAsociados(int categoriaId);

        Task Crear(Categoria categoria);
        Task Actualizar(Categoria categoria);
        Task Eliminar(Categoria categoria);
    }
}
