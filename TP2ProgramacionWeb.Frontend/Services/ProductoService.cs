using TP2ProgramacionWeb.Frontend.DTO.Categoria.Response;
using TP2ProgramacionWeb.Frontend.DTO.Paginado.Request.Producto;
using TP2ProgramacionWeb.Frontend.DTO.Producto.Request;
using TP2ProgramacionWeb.Frontend.DTO.Producto.Response;
using TP2ProgramacionWeb.Frontend.Excepciones;
using TP2ProgramacionWeb.Frontend.Models;
using TP2ProgramacionWeb.Frontend.Models.ModeloAuxiliar;
using TP2ProgramacionWeb.Frontend.Models.ModeloAuxiliar.Query.Producto;
using TP2ProgramacionWeb.Frontend.Repositories;
using TP2ProgramacionWeb.Frontend.Utilidades;

namespace TP2ProgramacionWeb.Frontend.Services
{
    public class ProductoService
    {
        private readonly IProductoRepository _repo;
        private readonly ICategoriaRepository _categoriaRepository;

        public ProductoService(IProductoRepository repo, ICategoriaRepository categoriaRepository)
        {
            _repo = repo;
            _categoriaRepository = categoriaRepository;
        }

        public async Task<PaginadoResponse<ProductoListadoResponse>> ObtenerTodos(ParametroPaginacionProductoRequest parametros)
        {

                int numeroPagina = parametros.NumeroPagina is null || parametros.NumeroPagina < 1
                    ? 1
                    : parametros.NumeroPagina.Value;

                int tamanoPagina = parametros.TamanoPagina is null || parametros.TamanoPagina < 1
                    ? 20
                    : parametros.TamanoPagina > 50 ? 50 : parametros.TamanoPagina.Value;

                int? categoriaId = parametros.CategoriaId;

                string? direccion =
                    string.IsNullOrWhiteSpace(parametros.Direccion) ||
                    parametros.Direccion?.ToLower() is not ("asc" or "desc")
                        ? "desc"
                        : parametros.Direccion;

                string? buscar = parametros.Buscar?.Trim().ToLower();

                string? ordenarPor = string.IsNullOrWhiteSpace(parametros.OrdenarPor) ? null : parametros.OrdenarPor;

                ProductoQueryParametros productoQueryParametros = new ProductoQueryParametros
                {
                    NumeroPagina = numeroPagina,
                    TamanoPagina = tamanoPagina,
                    CategoriaId = categoriaId,
                    Buscar = buscar,
                    OrdenarPor = ordenarPor,
                    Direccion = direccion
                };

                PaginadoResponse<Producto> resultado = await _repo.ObtenerTodos(productoQueryParametros);

            List<ProductoListadoResponse> productos = resultado.Datos.Select(p => new ProductoListadoResponse(
                p.Id, p.Nombre, p.PrecioCompra, p.PrecioVenta, p.Stock, p.CategoriaId, p.Categoria.Nombre
            )).ToList();

            return new PaginadoResponse<ProductoListadoResponse>(
                    productos,
                    resultado.NumeroPagina,
                    resultado.TamanoPagina,
                    resultado.TotalRegistros
                );

        }

        public async Task<Producto> ObtenerPorId(int id)
        {
                Producto? producto = await _repo.ObtenerPorId(id);

                if (producto is null)
                {
                    throw new RecursoNoExisteException("No existe ningún producto con ese id.");
                }

                return producto;
        }

        public async Task<ProductoListadoResponse> Crear(CrearProductoRequest dto)
        {
            if (!Validaciones.EstanDatosBien(dto.Nombre))
                throw new DatosLlegaronErradosException("El nombre del producto es obligatorio.");

            if (dto.PrecioCompra < 0 || dto.PrecioVenta < 0)
                throw new DatosLlegaronErradosException("Los precios no pueden ser negativos.");

            Categoria? categoria = await _categoriaRepository.ObtenerPorId(dto.CategoriaId);
            if (categoria is null)
                throw new RecursoNoExisteException("La categoría seleccionada no existe.");

            if (await _repo.ExistePorNombre(dto.Nombre.Trim()))
                throw new RecursoExistenteException("Ya existe un producto con ese nombre.");

            Producto producto = new Producto
            {
                Nombre = dto.Nombre.Trim(),
                PrecioCompra = dto.PrecioCompra,
                PrecioVenta = dto.PrecioVenta,
                Stock = 0,                       // el stock solo lo mueven los ingresos
                CategoriaId = categoria.Id
            };

            await _repo.Crear(producto);

            return new ProductoListadoResponse(producto.Id, producto.Nombre, producto.PrecioCompra,
                producto.PrecioVenta, producto.Stock, producto.CategoriaId, categoria.Nombre);
        }

        public async Task Actualizar(int id, ActualizarProductoRequest dto)
        {
            if (!Validaciones.EstanDatosBien(dto.Nombre))
                throw new DatosLlegaronErradosException("El nombre del producto es obligatorio.");

            if (dto.PrecioCompra < 0 || dto.PrecioVenta < 0)
                throw new DatosLlegaronErradosException("Los precios no pueden ser negativos.");

            Producto? producto = await _repo.ObtenerPorId(id);
            if (producto is null)
                throw new RecursoNoExisteException("No existe ningún producto con ese id.");

            if (await _categoriaRepository.ObtenerPorId(dto.CategoriaId) is null)
                throw new RecursoNoExisteException("La categoría seleccionada no existe.");

            string nombre = dto.Nombre.Trim();
            bool cambioElNombre = !string.Equals(producto.Nombre, nombre, StringComparison.OrdinalIgnoreCase);
            if (cambioElNombre && await _repo.ExistePorNombre(nombre))
                throw new RecursoExistenteException("Ya existe un producto con ese nombre.");

            producto.Nombre = nombre;
            producto.PrecioCompra = dto.PrecioCompra;
            producto.PrecioVenta = dto.PrecioVenta;
            producto.CategoriaId = dto.CategoriaId;

            await _repo.Actualizar(producto);
        }

        public async Task Eliminar(int id)
        {
            Producto? producto = await _repo.ObtenerPorId(id);
            if (producto is null)
                throw new RecursoNoExisteException("No existe ningún producto con ese id.");

            producto.Eliminado = true;           // borrado lógico: los ingresos históricos siguen intactos
            await _repo.Actualizar(producto);
        }
    }
}
