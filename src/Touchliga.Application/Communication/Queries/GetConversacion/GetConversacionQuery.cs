using MediatR;
using Touchliga.Application.Communication.DTOs;

namespace Touchliga.Application.Communication.Queries.GetConversacion;

public sealed record GetConversacionQuery(long OtroUsuarioId) : IRequest<IReadOnlyList<MensajeDto>>;
