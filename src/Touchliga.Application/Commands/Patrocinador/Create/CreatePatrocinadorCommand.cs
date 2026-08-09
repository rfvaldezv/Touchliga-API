using MediatR;

namespace Touchliga.Application.Commands.Patrocinador.Create;

public sealed record CreatePatrocinadorCommand(
    string Codigo,
    string Nombre,
    string Descripcion,
    string ImagenUrl,
    string? EnlaceUrl,
    int Orden,
    bool Activo
)
    : IRequest<long>;
