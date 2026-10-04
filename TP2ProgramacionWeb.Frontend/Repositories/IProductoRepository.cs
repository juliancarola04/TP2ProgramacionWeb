using TP2ProgramacionWeb.Frontend.Models;
using TP2ProgramacionWeb.Frontend.Models.ModeloAuxiliar;
using TP2ProgramacionWeb.Frontend.Models.ModeloAuxiliar.Query.Producto;

namespace TP2ProgramacionWeb.Frontend.Repositories
{
    public interface IProductoRepository
    {
        Task<PaginadoResponse<Producto>> ObtenerTodos(ProductoQueryParametros parametros);
        Task<Producto?> ObtenerPorId(int id);
        Task<bool> ExistePorNombre(string nombre);
        Task<bool> Existe(int id);
        Task Crear(Producto producto);
        Task Actualizar(Producto producto);
    }
}
