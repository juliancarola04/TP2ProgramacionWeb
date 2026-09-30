namespace TP2ProgramacionWeb.Frontend.DTO.Proveedor.Request;

public record ActualizarProveedorRequest(
    string RazonSocial,
    string Cuit,
    string Direccion,
    string Email,
    string Telefono);