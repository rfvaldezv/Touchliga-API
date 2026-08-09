using MediatR;
using Touchliga.Application.DTOs;

namespace Touchliga.Application.Queries.Pago.GetEstatus;

/// <summary>Todos los usuarios y si ya pagaron la cuota de esa temporada — para administración.</summary>
public sealed record GetEstatusPagosQuery(long TemporadaId) : IRequest<IReadOnlyList<EstatusPagoDto>>;
