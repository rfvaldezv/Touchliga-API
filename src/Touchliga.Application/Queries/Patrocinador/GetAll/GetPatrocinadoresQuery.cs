using MediatR;
using Touchliga.Application.DTOs;

namespace Touchliga.Application.Queries.Patrocinador.GetAll;

/// <summary>Todos, activos e inactivos — para administración.</summary>
public sealed record GetPatrocinadoresQuery() : IRequest<IReadOnlyList<PatrocinadorDto>>;
