using MediatR;
using Touchliga.Application.DTOs;

namespace Touchliga.Application.Queries.Premio.GetGanadoresFinales;

public sealed record GetGanadoresFinalesQuery(long TemporadaId) : IRequest<IReadOnlyList<GanadorPremioDto>>;
