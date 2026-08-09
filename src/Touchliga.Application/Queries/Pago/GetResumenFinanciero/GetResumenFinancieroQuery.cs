using MediatR;
using Touchliga.Application.DTOs;

namespace Touchliga.Application.Queries.Pago.GetResumenFinanciero;

public sealed record GetResumenFinancieroQuery(long TemporadaId) : IRequest<ResumenFinancieroDto>;
