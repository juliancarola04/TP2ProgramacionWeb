using MudBlazor;
using TP2ProgramacionWeb.Frontend.Excepciones;

namespace TP2ProgramacionWeb.Frontend.Utilidades;

public class ManejadorErroresUi
{
    private readonly ISnackbar _snackbar;
    private readonly ILogger<ManejadorErroresUi> _logger;

    public ManejadorErroresUi(ISnackbar snackbar, ILogger<ManejadorErroresUi> logger)
    {
        _snackbar = snackbar;
        _logger = logger;
    }

    // Ejecuta una acción y avisa por Snackbar. Devuelve true si salió bien.
    public async Task<bool> EjecutarAsync(Func<Task> accion, string? mensajeExito = null)
    {
        try
        {
            await accion();
            if (mensajeExito is not null) _snackbar.Add(mensajeExito, Severity.Success);
            return true;
        }
        catch (Exception ex) when (ex is RecursoNoExisteException
                                      or RecursoExistenteException
                                      or DatosLlegaronErradosException)
        {
            _snackbar.Add(ex.Message, Severity.Warning);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error no controlado en la interfaz");
            _snackbar.Add("Ocurrió un error inesperado. Intentá nuevamente.", Severity.Error);
            return false;
        }
    }

    // Igual, pero para acciones que devuelven un valor (default si falló).
    public async Task<T?> ObtenerAsync<T>(Func<Task<T>> accion)
    {
        T? resultado = default;
        await EjecutarAsync(async () => { resultado = await accion(); });
        return resultado;
    }
}