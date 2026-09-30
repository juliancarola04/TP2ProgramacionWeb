namespace TP2ProgramacionWeb.Frontend.DTO.Proveedor.Response;

public record ObtenerProveedorResponse(
    int Id,
    string RazonSocial,
    string Cuit,
    string Direccion,
    string Email,
    string Telefono);