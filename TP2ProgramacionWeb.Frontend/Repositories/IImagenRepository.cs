using TP2ProgramacionWeb.Frontend.Models;

namespace TP2ProgramacionWeb.Frontend.Repositories;

public interface IImagenRepository
{
    Task<Imagen?> ObtenerPorProductoId(int productoId);
    Task Crear(Imagen imagen);
    Task Eliminar(Imagen imagen);
}
