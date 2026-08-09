using MediatR;

namespace Touchliga.Application.Users.Commands.EditarInfoUsuario;

public sealed record EditarInfoUsuarioCommand(
    long UsuarioId,
    string Nombre,
    string Apellidos,
    string Telefono,
    string Correo,
    long? CiudadId,
    long? PaisId,
    long? EstadoId
)
    : IRequest<Unit>;
