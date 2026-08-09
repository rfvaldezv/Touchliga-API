using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.Communication.DTOs;
using Touchliga.Application.Communication.Queries.GetAnuncios;
using Touchliga.Application.Common.Interfaces;

namespace Touchliga.Application.Handlers.Communication.GetAnuncios;

public sealed class GetAnunciosQueryHandler : IRequestHandler<GetAnunciosQuery, IReadOnlyList<AnuncioDto>>
{
    private readonly IAnuncioRepository _anuncios;
    private readonly IUsuarioRepository _usuarios;
    private readonly IReaccionAnuncioRepository _reacciones;
    private readonly ICurrentUserService _currentUser;

    public GetAnunciosQueryHandler(
        IAnuncioRepository anuncios,
        IUsuarioRepository usuarios,
        IReaccionAnuncioRepository reacciones,
        ICurrentUserService currentUser)
    {
        _anuncios = anuncios;
        _usuarios = usuarios;
        _reacciones = reacciones;
        _currentUser = currentUser;
    }

    public async Task<IReadOnlyList<AnuncioDto>> Handle(GetAnunciosQuery request, CancellationToken cancellationToken)
    {
        var anuncios = await _anuncios.ObtenerTodosAsync(cancellationToken);
        var usuarios = await _usuarios.ObtenerTodosAsync();
        var nombres = usuarios.ToDictionary(u => u.Id, u => u.Nombre + " " + u.Apellidos);

        var anuncioIds = anuncios.Select(a => a.Id).ToList();
        var todasLasReacciones = await _reacciones.ObtenerPorAnunciosAsync(anuncioIds, cancellationToken);
        var reaccionesPorAnuncio = todasLasReacciones.GroupBy(r => r.AnuncioId)
            .ToDictionary(g => g.Key, g => g.ToList());

        return anuncios
            .OrderByDescending(a => a.FechaPublicacion)
            .Select(a =>
            {
                var reaccionesDeEste = reaccionesPorAnuncio.GetValueOrDefault(a.Id, []);
                var miReaccion = reaccionesDeEste.FirstOrDefault(r => r.UsuarioId == _currentUser.UserId);

                return new AnuncioDto
                {
                    Id = a.Id,
                    Titulo = a.Titulo,
                    Contenido = a.Contenido,
                    ImagenUrl = a.ImagenUrl,
                    AutorNombre = nombres.TryGetValue(a.UsuarioAutorId, out var n) ? n : "Organizador",
                    FechaPublicacion = a.FechaPublicacion,
                    Reacciones = reaccionesDeEste
                        .GroupBy(r => r.Emoji)
                        .ToDictionary(g => g.Key, g => g.Count()),
                    MiReaccion = miReaccion?.Emoji,
                };
            })
            .ToList();
    }
}
