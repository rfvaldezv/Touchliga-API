using MediatR;

namespace Touchliga.Application.Commands.PushToken.Eliminar;

/// <summary>Se llama al cerrar sesión, para dejar de mandarle push a ese dispositivo.</summary>
public sealed record EliminarPushTokenCommand(string Token) : IRequest<Unit>;
