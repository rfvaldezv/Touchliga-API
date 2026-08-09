using MediatR;

namespace Touchliga.Application.Communication.Commands.EditarAnuncio;

public sealed record EditarAnuncioCommand(
    long Id,
    string Titulo,
    string Contenido,
    bool ReenviarPush,
    string? ImagenUrl = null
)
    : IRequest<Unit>;
