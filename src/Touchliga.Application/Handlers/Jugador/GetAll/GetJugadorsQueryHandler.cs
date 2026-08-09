using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.DTOs;
using Touchliga.Application.Queries.Jugador.GetAll;

namespace Touchliga.Application.Handlers.Jugador.GetAll;

public sealed class GetJugadorsQueryHandler : IRequestHandler<GetJugadorsQuery, IReadOnlyList<JugadorDto>>
{
    private readonly IJugadorRepository _repository;

    public GetJugadorsQueryHandler(
        IJugadorRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<JugadorDto>> Handle(
        GetJugadorsQuery request,
        CancellationToken cancellationToken)
    {
        var entidades = await _repository.ObtenerTodosAsync(cancellationToken);

        return entidades
            .Select(entidad => new JugadorDto
            {
                Id = entidad.Id,
                Codigo = entidad.Codigo,
                Nombre = entidad.Nombre,
                Descripcion = entidad.Descripcion ?? string.Empty,
                Activo = entidad.Activo
            })
            .ToList();
    }
}
