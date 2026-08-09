using MediatR;

namespace Touchliga.Application.Communication.Commands.MarcarConversacionLeida;

public sealed record MarcarConversacionLeidaCommand(long OtroUsuarioId) : IRequest<Unit>;
