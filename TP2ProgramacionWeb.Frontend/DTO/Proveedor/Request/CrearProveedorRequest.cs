namespace TP2ProgramacionWeb.Frontend.DTO.Proveedor.Request;

public record CrearProveedorRequest(
    string RazonSocial,
    string Cuit,
    string Direccion,
    string Email,
    string Telefono);