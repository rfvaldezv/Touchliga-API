using MediatR;
using Touchliga.Application.DTOs;

namespace Touchliga.Application.Queries.Reportes.GetRanking;

public sealed record GetRankingQuery(long TemporadaId) : IRequest<IReadOnlyList<RankingDto>>;
