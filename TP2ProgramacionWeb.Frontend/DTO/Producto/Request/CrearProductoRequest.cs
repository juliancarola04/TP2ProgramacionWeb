namespace TP2ProgramacionWeb.Frontend.DTO.Producto.Request;

public record CrearProductoRequest(
    string Nombre,
    decimal PrecioCompra,
    decimal PrecioVenta,
    int Stock,
    int CategoriaId);