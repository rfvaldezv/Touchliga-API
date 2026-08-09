using MediatR;

namespace Touchliga.Application.Commands.Premio.Decidir;

/// <summary>estado: "Aprobado" o "Denegado". montoAjustado: opcional,
/// si el responsable quiere pagar un monto distinto al sugerido.
/// motivo: opcional, útil sobre todo al denegar (ej. "mala
/// actitud").</summary>
public sealed record DecidirPremioCommand(
    string Ambito,
    long ReferenciaId,
    long UsuarioId,
    string Estado,
    decimal? MontoAjustado,
    string? Motivo
) : IRequest<Unit>;
