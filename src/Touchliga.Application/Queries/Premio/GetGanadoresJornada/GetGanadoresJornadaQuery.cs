using MediatR;
using Touchliga.Application.DTOs;

namespace Touchliga.Application.Queries.Premio.GetGanadoresJornada;

public sealed record GetGanadoresJornadaQuery(long JornadaId) : IRequest<IReadOnlyList<GanadorPremioDto>>;
