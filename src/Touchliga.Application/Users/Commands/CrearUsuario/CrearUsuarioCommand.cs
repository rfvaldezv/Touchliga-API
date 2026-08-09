using MediatR;

namespace Touchliga.Application.Users.Commands.CrearUsuario;

/// <summary>
/// Alta de un participante nuevo. La captura un administrador con
/// los datos obligatorios: nombre, apellidos, teléfono (para el
/// grupo de WhatsApp), correo, quién lo invitó, ciudad, país y estado.
/// </summary>
public sealed record CrearUsuarioCommand(
    string Nombre,
    string Apellidos,
    string Telefono,
    string Correo,
    string Password,
    string Sexo,
    long InvitadoPorId,
    long CiudadId,
    long PaisId,
    long EstadoId
)
    : IRequest<long>;
