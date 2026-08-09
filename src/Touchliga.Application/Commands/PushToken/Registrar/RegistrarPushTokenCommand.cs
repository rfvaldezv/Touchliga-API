using MediatR;

namespace Touchliga.Application.Commands.PushToken.Registrar;

public sealed record RegistrarPushTokenCommand(string Token, string Plataforma) : IRequest<Unit>;
