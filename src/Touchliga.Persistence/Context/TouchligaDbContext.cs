using Touchliga.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Touchliga.Persistence.Context;
public sealed class TouchligaDbContext : DbContext
{
    public TouchligaDbContext(DbContextOptions<TouchligaDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();

    public DbSet<CredencialAlterna> CredencialesAlternas => Set<CredencialAlterna>();
    public DbSet<Rol> Roles => Set<Rol>();
    public DbSet<Permiso> Permisos => Set<Permiso>();
    public DbSet<Sesion> Sesiones => Set<Sesion>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<UsuarioRol> UsuarioRoles => Set<UsuarioRol>();
    public DbSet<Deporte> Deportes => Set<Deporte>();
    public DbSet<Equipo> Equipos => Set<Equipo>();
    public DbSet<Liga> Ligas => Set<Liga>();
    public DbSet<Temporada> Temporadas => Set<Temporada>();
    public DbSet<Pais> Pais => Set<Pais>();
    public DbSet<Pais> Paises => Set<Pais>();
    public DbSet<Estado> Estados => Set<Estado>();
    public DbSet<Ciudad> Ciudads => Set<Ciudad>();
    public DbSet<Cancha> Canchas => Set<Cancha>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    // Arbitro removido -- Touchliga no lo usa
    public DbSet<Jugador> Jugadors => Set<Jugador>();
    public DbSet<Jornada> Jornadas => Set<Jornada>();
    public DbSet<Partido> Partidos => Set<Partido>();
    public DbSet<Pronostico> Pronosticos => Set<Pronostico>();
    public DbSet<Anuncio> Anuncios => Set<Anuncio>();
    public DbSet<ReaccionAnuncio> ReaccionesAnuncio => Set<ReaccionAnuncio>();
    public DbSet<Mensaje> Mensajes => Set<Mensaje>();
    public DbSet<Patrocinador> Patrocinadores => Set<Patrocinador>();
    public DbSet<Pago> Pagos => Set<Pago>();
    public DbSet<ConfiguracionPremio> ConfiguracionesPremio => Set<ConfiguracionPremio>();

    public DbSet<ConfiguracionSmtp> ConfiguracionesSmtp => Set<ConfiguracionSmtp>();
    public DbSet<PremioOtorgado> PremiosOtorgados => Set<PremioOtorgado>();
    public DbSet<Archivo> Archivos => Set<Archivo>();
    public DbSet<PushToken> PushTokens => Set<PushToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TouchligaDbContext).Assembly);
    }
}