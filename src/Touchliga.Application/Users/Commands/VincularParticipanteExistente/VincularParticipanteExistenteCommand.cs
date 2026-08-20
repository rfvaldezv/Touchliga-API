using MediatR;

namespace Touchliga.Application.Users.Commands.VincularParticipanteExistente;

/// <summary>Toma a un participante YA REGISTRADO (con su correo y
/// contraseña ya existentes) y lo vincula como segundo acceso de
/// otro -- sin pedir datos nuevos. El vinculado deja de jugar por su
/// cuenta (EsCuentaVinculada = true); su correo+contraseña de
/// siempre ahora lo llevan a la cuenta del participante objetivo.</summary>
public sealed record VincularParticipanteExistenteCommand(
    long UsuarioObjetivoId,
    long UsuarioAVincularId
) : IRequest<Unit>;
