using MediatR;
using Touchliga.Application.DTOs;

namespace Touchliga.Application.Queries.Patrocinador.GetActivos;

/// <summary>Solo los activos, ordenados — para mostrar el banner rotativo.</summary>
public sealed record GetPatrocinadoresActivosQuery() : IRequest<IReadOnlyList<PatrocinadorDto>>;
