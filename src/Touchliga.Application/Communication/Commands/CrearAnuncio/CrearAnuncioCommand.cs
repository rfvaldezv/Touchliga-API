using MediatR;

namespace Touchliga.Application.Communication.Commands.CrearAnuncio;

public sealed record CrearAnuncioCommand(string Titulo, string Contenido, string? ImagenUrl = null) : IRequest<long>;
