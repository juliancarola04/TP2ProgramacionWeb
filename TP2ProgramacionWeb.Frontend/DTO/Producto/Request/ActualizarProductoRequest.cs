namespace TP2ProgramacionWeb.Frontend.DTO.Producto.Request;

public record ActualizarProductoRequest(
    string Nombre,
    decimal PrecioCompra,
    decimal PrecioVenta,
    int CategoriaId);