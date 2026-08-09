using MediatR;
using Touchliga.Application.Communication.DTOs;

namespace Touchliga.Application.Communication.Queries.GetTodosLosParticipantes;

/// <summary>Todos los participantes activos (menos yo mismo) — para
/// poder iniciar una conversación con cualquiera, no solo con un
/// administrador/capturador.</summary>
public sealed record GetTodosLosParticipantesQuery() : IRequest<IReadOnlyList<ContactoDto>>;
