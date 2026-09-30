namespace TP2ProgramacionWeb.Frontend.DTO.Proveedor.Response;

public record CrearProveedorResponse(
    int Id,
    string RazonSocial,
    string Cuit,
    string Direccion,
    string Email,
    string Telefono);