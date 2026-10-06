using TP2ProgramacionWeb.Frontend.DTO.Ingreso.Request;
using TP2ProgramacionWeb.Frontend.DTO.Ingreso.Response;
using TP2ProgramacionWeb.Frontend.DTO.Paginado.Request.Ingreso;
using TP2ProgramacionWeb.Frontend.Excepciones;
using TP2ProgramacionWeb.Frontend.Models;
using TP2ProgramacionWeb.Frontend.Models.ModeloAuxiliar;
using TP2ProgramacionWeb.Frontend.Models.ModeloAuxiliar.Query.Ingreso;
using TP2ProgramacionWeb.Frontend.Repositories;

namespace TP2ProgramacionWeb.Frontend.Services;

public class IngresoService
{
    private readonly IIngresoRepository _repo;
    private readonly IProductoRepository _productoRepo;
    private readonly IProveedorRepository _proveedorRepo;

    public IngresoService(IIngresoRepository repo, IProductoRepository productoRepo, IProveedorRepository proveedorRepo)
    {
        _repo = repo;
        _productoRepo = productoRepo;
        _proveedorRepo = proveedorRepo;
    }

    public async Task<PaginadoResponse<IngresoListadoResponse>> ObtenerTodos(ParametroPaginacionIngresoRequest parametros)
    {
            int numeroPagina = parametros.NumeroPagina is null || parametros.NumeroPagina < 1
                ? 1
                : parametros.NumeroPagina.Value;

            int tamanoPagina = parametros.TamanoPagina is null || parametros.TamanoPagina < 1
                ? 20
                : parametros.TamanoPagina > 50 ? 50 : parametros.TamanoPagina.Value;

            int? proveedorId = parametros.ProveedorId;
            bool? anulado = parametros.Anulado;

            string? direccion =
                string.IsNullOrWhiteSpace(parametros.Direccion) ||
                parametros.Direccion?.ToLower() is not ("asc" or "desc")
                    ? "desc"
                    : parametros.Direccion;

            string? buscar = parametros.Buscar?.Trim().ToLower();

            string? ordenarPor = string.IsNullOrWhiteSpace(parametros.OrdenarPor) ? "fecha" : parametros.OrdenarPor;

            IngresoQueryParametros ingresoQueryParametros = new IngresoQueryParametros
            {
                NumeroPagina = numeroPagina,
                TamanoPagina = tamanoPagina,
                ProveedorId = proveedorId,
                Anulado = anulado,
                Buscar = buscar,
                OrdenarPor = ordenarPor,
                Direccion = direccion
            };

            PaginadoResponse<Ingreso> resultado = await _repo.ObtenerTodos(ingresoQueryParametros);

            List<IngresoListadoResponse> ingresos = resultado.Datos.Select(i => new IngresoListadoResponse(
                i.Id, i.Fecha, i.Total, i.ProveedorId, i.Proveedor.RazonSocial, i.Anulado
            )).ToList();

            return new PaginadoResponse<IngresoListadoResponse>(
                ingresos,
                resultado.NumeroPagina,
                resultado.TamanoPagina,
                resultado.TotalRegistros
            );
    }

    public async Task<IngresoResponse> ObtenerPorId(int id)
    {

            Ingreso? ingreso = await _repo.ObtenerPorId(id);

            if (ingreso is null)
            {
                throw new RecursoNoExisteException("No existe ningún ingreso con ese id.");
            }

            return MapearADto(ingreso);

    }

    public async Task<IngresoResponse> Crear(CrearIngresoRequest dto, int usuarioId)
    {
        // 1) Validaciones de forma (no tocan la base)
        if (dto.Items is null || dto.Items.Count == 0)
            throw new DatosLlegaronErradosException("El ingreso debe tener al menos un producto.");

        if (dto.Items.Any(i => i.Cantidad <= 0))
            throw new DatosLlegaronErradosException("La cantidad de cada producto debe ser mayor a cero.");

        if (dto.Items.Any(i => i.PrecioUnitario <= 0))
            throw new DatosLlegaronErradosException("El precio unitario de cada producto debe ser mayor a cero.");

        bool hayDuplicados = dto.Items
            .GroupBy(i => i.ProductoId)
            .Any(g => g.Count() > 1);

        if (hayDuplicados)
            throw new DatosLlegaronErradosException(
                "Hay un producto repetido en la lista de items. Combiná las cantidades en un solo ítem antes de enviar.");

        // 2) Validaciones de existencia: se carga todo, sin modificar nada todavía
        Proveedor? proveedor = await _proveedorRepo.BuscarPorId(dto.ProveedorId);
        if (proveedor is null)
            throw new RecursoNoExisteException("No existe ningún proveedor con ese id.");

        List<(Producto Producto, CrearIngresoItemRequest Item)> items = new();

        foreach (CrearIngresoItemRequest item in dto.Items)
        {
            Producto? producto = await _productoRepo.ObtenerPorId(item.ProductoId);

            if (producto is null)
                throw new RecursoNoExisteException($"No existe ningún producto con id {item.ProductoId}.");

            items.Add((producto, item));
        }

        // 3) Recién acá se modifica el stock y el costo vigente
        List<DetalleIngreso> detalles = new();
        decimal total = 0;

        foreach ((Producto producto, CrearIngresoItemRequest item) in items)
        {
            producto.Stock += item.Cantidad;
            producto.PrecioCompra = item.PrecioUnitario; // costo vigente en el catálogo

            total += item.PrecioUnitario * item.Cantidad;

            detalles.Add(new DetalleIngreso
            {
                ProductoId = producto.Id,
                Cantidad = item.Cantidad,
                PrecioUnitario = item.PrecioUnitario
            });
        }

        Ingreso ingreso = new Ingreso
        {
            Fecha = DateTime.UtcNow,
            Total = total,
            ProveedorId = dto.ProveedorId,
            UsuarioId = usuarioId,
            DetallesIngresos = detalles
        };

        await _repo.Crear(ingreso); // si falla, el repo limpia el ChangeTracker (ver mensaje anterior)

        Ingreso? ingresoCompleto = await _repo.ObtenerPorId(ingreso.Id);
        return MapearADto(ingresoCompleto!);
    }
    public async Task Anular(int id)
    {

            Ingreso? ingreso = await _repo.ObtenerParaAnular(id);

            if (ingreso is null)
            {
                throw new RecursoNoExisteException("No existe ningún ingreso con ese id.");
            }

            if (ingreso.Anulado)
            {
                throw new DatosLlegaronErradosException("El ingreso ya se encuentra anulado.");
            }

            // Antes de tocar nada, verificamos que revertir el stock no deje ningún producto en negativo.
            foreach (DetalleIngreso detalle in ingreso.DetallesIngresos)
            {
                if (detalle.Producto.Stock < detalle.Cantidad)
                {
                    throw new DatosLlegaronErradosException(
                        $"No se puede anular: el producto '{detalle.Producto.Nombre}' (id {detalle.Producto.Id}) " +
                        $"tiene stock actual {detalle.Producto.Stock}, menor a las {detalle.Cantidad} unidades a revertir.");
                }
            }

            foreach (DetalleIngreso detalle in ingreso.DetallesIngresos)
            {
                detalle.Producto.Stock -= detalle.Cantidad;
            }

            ingreso.Anulado = true;

            await _repo.GuardarCambios();

    }

    private static IngresoResponse MapearADto(Ingreso ingreso)
    {
        List<IngresoDetalleResponse> detalles = ingreso.DetallesIngresos.Select(d => new IngresoDetalleResponse(
            d.ProductoId,
            d.Producto.Nombre,
            d.Cantidad,
            d.PrecioUnitario,
            d.PrecioUnitario * d.Cantidad
        )).ToList();

        return new IngresoResponse(
            ingreso.Id,
            ingreso.Fecha,
            ingreso.Total,
            ingreso.ProveedorId,
            ingreso.Proveedor.RazonSocial,
            ingreso.UsuarioId,
            ingreso.Usuario.Username,
            ingreso.Anulado,
            detalles);
    }
}
