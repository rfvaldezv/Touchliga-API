using MediatR;
using Touchliga.Application.DTOs;

namespace Touchliga.Application.Queries.Pago.GetMio;

/// <summary>Mi propio estatus de pago para esa temporada.</summary>
public sealed record GetMiPagoQuery(long TemporadaId) : IRequest<ResumenPagoDto>;
