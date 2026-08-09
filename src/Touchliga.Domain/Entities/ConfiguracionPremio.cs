using Touchliga.Domain.Common;
using Touchliga.Domain.Exceptions;

namespace Touchliga.Domain.Entities;

/// <summary>
/// Un premio configurado para una posición específica — ya sea "por
/// jornada" (se reparte cada jornada, normalmente 1°-3° lugar) o
/// "final de temporada" (se reparte una sola vez al terminar,
/// normalmente 1°-10° lugar). Puede ser efectivo o en especie
/// (regalo) — en cualquier caso lleva un Monto, que para un regalo
/// representa su valor equivalente (necesario para poder repartir
/// bien en caso de empate entre posiciones).
/// </summary>
public sealed class ConfiguracionPremio : AggregateRoot
{
    private ConfiguracionPremio()
    {
    }

    public long TemporadaId { get; private set; }

    /// <summary>"Jornada" o "Final".</summary>
    public string Ambito { get; private set; } = string.Empty;

    /// <summary>1 = primer lugar, 2 = segundo, etc.</summary>
    public int Posicion { get; private set; }

    /// <summary>"Efectivo" o "Especie".</summary>
    public string TipoPremio { get; private set; } = string.Empty;

    public decimal Monto { get; private set; }

    /// <summary>Qué es el regalo, si TipoPremio es "Especie".</summary>
    public string? Descripcion { get; private set; }

    public static ConfiguracionPremio Crear(
        long temporadaId,
        string ambito,
        int posicion,
        string tipoPremio,
        decimal monto,
        string? descripcion,
        long usuarioId)
    {
        Validar(ambito, posicion, tipoPremio, monto);

        return new ConfiguracionPremio
        {
            TemporadaId = temporadaId,
            Ambito = ambito,
            Posicion = posicion,
            TipoPremio = tipoPremio,
            Monto = monto,
            Descripcion = string.IsNullOrWhiteSpace(descripcion) ? null : descripcion.Trim(),
            UsuarioAltaId = usuarioId,
            FechaAlta = DateTime.UtcNow,
            Activo = true
        };
    }

    public void Editar(string tipoPremio, decimal monto, string? descripcion, long usuarioId)
    {
        Validar(Ambito, Posicion, tipoPremio, monto);

        TipoPremio = tipoPremio;
        Monto = monto;
        Descripcion = string.IsNullOrWhiteSpace(descripcion) ? null : descripcion.Trim();

        MarcarModificado(usuarioId);
    }

    private static void Validar(string ambito, int posicion, string tipoPremio, decimal monto)
    {
        if (ambito != "Jornada" && ambito != "Final")
            throw new DomainException("El ámbito del premio debe ser \"Jornada\" o \"Final\".");

        if (posicion <= 0)
            throw new DomainException("La posición debe ser mayor a cero.");

        if (tipoPremio != "Efectivo" && tipoPremio != "Especie")
            throw new DomainException("El tipo de premio debe ser \"Efectivo\" o \"Especie\".");

        if (monto <= 0)
            throw new DomainException("El monto/valor del premio debe ser mayor a cero.");
    }
}
