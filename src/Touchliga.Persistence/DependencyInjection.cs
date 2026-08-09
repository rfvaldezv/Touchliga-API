using Touchliga.Domain.Interfaces;
using Touchliga.Persistence.Context;
using Touchliga.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Touchliga.Persistence;
public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TouchligaDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<ISesionRepository, SesionRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<ITemporadaRepository, TemporadaRepository>();
        services.AddScoped<IPaisRepository, PaisRepository>();
        services.AddScoped<IEstadoRepository, EstadoRepository>();
        services.AddScoped<ICiudadRepository, CiudadRepository>();
        services.AddScoped<ICanchaRepository, CanchaRepository>();
        services.AddScoped<ICategoriaRepository, CategoriaRepository>();
        // Arbitro removido -- Touchliga no lo usa (no hay concepto de "árbitros" a capturar aquí)
        services.AddScoped<IJugadorRepository, JugadorRepository>();
        services.AddScoped<IJornadaRepository, JornadaRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IDeporteRepository, DeporteRepository>();
        services.AddScoped<IEquipoRepository, EquipoRepository>();
        services.AddScoped<ILigaRepository, LigaRepository>();
        services.AddScoped<IPartidoRepository, PartidoRepository>();
        services.AddScoped<IPronosticoRepository, PronosticoRepository>();
        services.AddScoped<IPosicionesRepository, PosicionesRepository>();
        services.AddScoped<IRolRepository, RolRepository>();
        services.AddScoped<IUsuarioRolRepository, UsuarioRolRepository>();
        services.AddScoped<IAnuncioRepository, AnuncioRepository>();
        services.AddScoped<IReaccionAnuncioRepository, ReaccionAnuncioRepository>();
        services.AddScoped<IMensajeRepository, MensajeRepository>();
        services.AddScoped<IPatrocinadorRepository, PatrocinadorRepository>();
        services.AddScoped<IPagoRepository, PagoRepository>();
        services.AddScoped<IConfiguracionPremioRepository, ConfiguracionPremioRepository>();
        services.AddScoped<IPremioOtorgadoRepository, PremioOtorgadoRepository>();
        services.AddScoped<IArchivoRepository, ArchivoRepository>();
        services.AddScoped<IPushTokenRepository, PushTokenRepository>();
        services.AddScoped<IReportesRepository, ReportesRepository>();
        return services;
    }
}