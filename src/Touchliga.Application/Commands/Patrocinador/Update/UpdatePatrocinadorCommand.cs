using MediatR;

namespace Touchliga.Application.Commands.Patrocinador.Update;

public sealed record UpdatePatrocinadorCommand(
    long Id,
    string Nombre,
    string Descripcion,
    string ImagenUrl,
    string? EnlaceUrl,
    int Orden,
    bool Activo
)
    : IRequest<long>;
