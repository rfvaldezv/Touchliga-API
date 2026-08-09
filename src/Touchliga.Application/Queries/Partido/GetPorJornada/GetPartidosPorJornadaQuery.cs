using MediatR;
using Touchliga.Application.DTOs;

namespace Touchliga.Application.Queries.Partido.GetPorJornada;

public sealed record GetPartidosPorJornadaQuery(long JornadaId)
    : IRequest<IReadOnlyList<PartidoDto>>;
