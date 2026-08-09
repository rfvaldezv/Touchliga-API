using MediatR;
using Touchliga.Domain.Enums;

namespace Touchliga.Application.Users.Commands.CambiarEstatusUsuario;

public sealed record CambiarEstatusUsuarioCommand(long UsuarioId, EstatusParticipante Estatus) : IRequest<Unit>;
