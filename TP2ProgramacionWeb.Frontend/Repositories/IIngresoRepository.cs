using TP2ProgramacionWeb.Frontend.Models;
using TP2ProgramacionWeb.Frontend.Models.ModeloAuxiliar;
using TP2ProgramacionWeb.Frontend.Models.ModeloAuxiliar.Query.Ingreso;

namespace TP2ProgramacionWeb.Frontend.Repositories
{
    public interface IIngresoRepository
    {
        Task<PaginadoResponse<Ingreso>> ObtenerTodos(IngresoQueryParametros parametros);
        Task<Ingreso?> ObtenerPorId(int id);
        Task<Ingreso?> ObtenerParaAnular(int id);
        Task Crear(Ingreso ingreso);
        Task GuardarCambios();
    }
}
