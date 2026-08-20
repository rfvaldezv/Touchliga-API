using MediatR;

namespace Touchliga.Application.Users.Commands.AgregarCredencialAlterna;

/// <summary>Registra (o reemplaza, si ya existía una) un segundo
/// correo+contraseña que puede iniciar sesión COMO este mismo
/// participante -- mismos pronósticos, mismos puntos, mismo Id.</summary>
public sealed record AgregarCredencialAlternaCommand(long UsuarioId, string Correo, string Password) : IRequest<Unit>;
