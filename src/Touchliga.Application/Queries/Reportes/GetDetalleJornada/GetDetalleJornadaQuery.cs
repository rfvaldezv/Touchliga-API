using MediatR;
using Touchliga.Application.DTOs;

namespace Touchliga.Application.Queries.Reportes.GetDetalleJornada;

public sealed record GetDetalleJornadaQuery(long JornadaId) : IRequest<IReadOnlyList<DetalleJornadaDto>>;
