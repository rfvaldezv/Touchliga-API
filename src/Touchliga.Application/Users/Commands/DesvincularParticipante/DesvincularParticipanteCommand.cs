using MediatR;

namespace Touchliga.Application.Users.Commands.DesvincularParticipante;

/// <summary>Deshace la vinculación de VincularParticipanteExistente
/// -- quita la credencial alterna que apuntaba a la cuenta objetivo,
/// y el participante recupera su propio correo/contraseña como vía
/// de acceso normal a SU PROPIA cuenta otra vez.</summary>
public sealed record DesvincularParticipanteCommand(long UsuarioObjetivoId, long UsuarioVinculadoId) : IRequest<Unit>;
