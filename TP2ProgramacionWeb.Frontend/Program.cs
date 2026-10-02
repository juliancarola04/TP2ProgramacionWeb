using System.Data.Common;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using TP2ProgramacionWeb.Frontend.Components;
using TP2ProgramacionWeb.Frontend.Data;
using TP2ProgramacionWeb.Frontend.Implementacion;
using TP2ProgramacionWeb.Frontend.Repositories;
using TP2ProgramacionWeb.Frontend.Services;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Iniciando la página...");

    
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();
    
    builder.Services.AddAuthorization();
    
    builder.Services.AddCascadingAuthenticationState();
    
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    builder.Services.AddDbContext<DataContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
    
    builder.Services.AddScoped<ProductoService>();
    builder.Services.AddScoped<CategoriaService>();
    builder.Services.AddScoped<ProveedorService>();
    builder.Services.AddScoped<IngresoService>();

    builder.Services.AddScoped<IProductoRepository, ProductoRepositoryPsqlEF>();
    builder.Services.AddScoped<ICategoriaRepository, CategoriaRepositoryPsqlEF>();
    builder.Services.AddScoped<IProveedorRepository, ProveedorRepositoryPsqlEF>();
    builder.Services.AddScoped<IIngresoRepository, IngresoRepositoryPsqlEF>();
    
    var app = builder.Build();
    
    var uploadsPath = Path.Combine(
        app.Environment.WebRootPath
        ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "uploads");

    if (!Directory.Exists(uploadsPath))
    {
        Directory.CreateDirectory(uploadsPath);
    }

    app.UseExceptionHandler(exceptionHandlerApp =>
    {
        exceptionHandlerApp.Run(async context =>
        {
            var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
            Exception? exception = exceptionFeature?.Error;

            var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            DbException? dbException = BuscarDbExceptionEnCadena(exception);

            if (dbException is not null)
            {
                // Logueamos la excepción ORIGINAL completa (con toda la cadena de InnerException incluida).
                logger.LogError(exception,
                    "Error de base de datos al procesar {Method} {Path}",
                    context.Request.Method, context.Request.Path);

                await context.Response.WriteAsync("\"Ocurrió un error interno en el servidor. Por favor, intentá nuevamente más tarde.\"");
            }
            else
            {
                logger.LogError(exception,
                    "Excepción no controlada al procesar {exception} {Path}",
                    context.Request.Method, context.Request.Path);

                await context.Response.WriteAsync("\"Ocurrió un error interno en el servidor.\"");
            }
        });
    });
    
    static DbException? BuscarDbExceptionEnCadena(Exception? ex)
    {
        while (ex is not null)
        {
            if (ex is DbException dbEx)
            {
                return dbEx;
            }
            ex = ex.InnerException;
            Console.WriteLine(ex.Message);
        }
        return null;
    }
    
    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
    app.UseHttpsRedirection();

    app.UseAntiforgery();
    
    app.UseAuthorization();

    app.MapStaticAssets();
    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Fallo crítico no controlado durante el inicio de la aplicación.");
}
finally
{
    Log.CloseAndFlush();
}
