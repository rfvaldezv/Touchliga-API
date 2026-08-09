namespace Touchliga.Domain.Interfaces;

public sealed record PosicionResumen(
    long UsuarioId,
    string Nombre,
    int Puntos,
    int Aciertos,
    int Pronosticos);

/// <summary>
/// Consulta de reporte (tabla de posiciones). No es un repositorio
/// de una entidad en particular, sino una vista agregada sobre
/// Pronostico + Partido + Jornada + Usuario.
/// </summary>
public interface IPosicionesRepository
{
    Task<IReadOnlyList<PosicionResumen>> ObtenerTablaPosicionesAsync(
        long temporadaId,
        CancellationToken cancellationToken = default);
}
