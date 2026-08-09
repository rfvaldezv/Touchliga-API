using Touchliga.Domain.Common;
using Touchliga.Domain.Exceptions;

namespace Touchliga.Domain.Entities;

/// <summary>
/// La DECISIÓN humana sobre un premio sugerido — el cálculo
/// automático (ver CalculadoraPremios) solo propone quién ganó y
/// cuánto; el responsable de finanzas es quien decide si de verdad
/// se paga (puede negarlo por cualquier motivo — mala actitud,
/// incumplimiento, etc.) o ajustar el monto antes de aprobarlo.
/// Sin una decisión registrada, un premio sugerido queda "Pendiente"
/// — nunca se asume aprobado solo.
/// </summary>
public sealed class PremioOtorgado : AggregateRoot
{
    private PremioOtorgado()
    {
    }

    /// <summary>"Jornada" o "Final".</summary>
    public string Ambito { get; private set; } = string.Empty;

    /// <summary>JornadaId si Ambito es "Jornada", TemporadaId si es "Final".</summary>
    public long ReferenciaId { get; private set; }

    public long UsuarioId { get; private set; }

    /// <summary>"Aprobado" o "Denegado".</summary>
    public string Estado { get; private set; } = string.Empty;

    /// <summary>Si es null, se paga el monto sugerido tal cual.</summary>
    public decimal? MontoAjustado { get; private set; }

    public string? Motivo { get; private set; }

    public long DecididoPorId { get; private set; }

    public DateTime FechaDecision { get; private set; }

    public static PremioOtorgado Decidir(
        string ambito,
        long referenciaId,
        long usuarioId,
        string estado,
        decimal? montoAjustado,
        string? motivo,
        long decididoPorId)
    {
        Validar(ambito, estado, montoAjustado);

        return new PremioOtorgado
        {
            Ambito = ambito,
            ReferenciaId = referenciaId,
            UsuarioId = usuarioId,
            Estado = estado,
            MontoAjustado = montoAjustado,
            Motivo = string.IsNullOrWhiteSpace(motivo) ? null : motivo.Trim(),
            DecididoPorId = decididoPorId,
            FechaDecision = DateTime.UtcNow,
            UsuarioAltaId = decididoPorId,
            FechaAlta = DateTime.UtcNow,
            Activo = true
        };
    }

    /// <summary>Para cuando el responsable cambia de opinión sobre una decisión ya tomada.</summary>
    public void Redecidir(string estado, decimal? montoAjustado, string? motivo, long decididoPorId)
    {
        Validar(Ambito, estado, montoAjustado);

        Estado = estado;
        MontoAjustado = montoAjustado;
        Motivo = string.IsNullOrWhiteSpace(motivo) ? null : motivo.Trim();
        DecididoPorId = decididoPorId;
        FechaDecision = DateTime.UtcNow;

        MarcarModificado(decididoPorId);
    }

    private static void Validar(string ambito, string estado, decimal? montoAjustado)
    {
        if (ambito != "Jornada" && ambito != "Final")
            throw new DomainException("El ámbito debe ser \"Jornada\" o \"Final\".");

        if (estado != "Aprobado" && estado != "Denegado")
            throw new DomainException("El estado debe ser \"Aprobado\" o \"Denegado\".");

        if (montoAjustado is not null && montoAjustado <= 0)
            throw new DomainException("El monto ajustado, si se captura, debe ser mayor a cero.");
    }
}
