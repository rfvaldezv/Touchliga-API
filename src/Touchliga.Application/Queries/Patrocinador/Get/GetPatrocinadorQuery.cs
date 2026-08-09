using MediatR;
using Touchliga.Application.DTOs;

namespace Touchliga.Application.Queries.Patrocinador.Get;

public sealed record GetPatrocinadorQuery(long Id) : IRequest<PatrocinadorDto>;
