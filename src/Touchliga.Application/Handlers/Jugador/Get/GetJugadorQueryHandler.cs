using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.DTOs;
using Touchliga.Application.Queries.Jugador.Get;

namespace Touchliga.Application.Handlers.Jugador.Get;

public sealed class GetJugadorQueryHandler : IRequestHandler<GetJugadorQuery, JugadorDto>
{
    private readonly IJugadorRepository _repository;

    public GetJugadorQueryHandler(
        IJugadorRepository repository)
    {
        _repository = repository;
    }

    public async Task<JugadorDto> Handle(
        GetJugadorQuery request,
        CancellationToken cancellationToken)
    {
        var entidad = await _repository.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Jugador");

        return new JugadorDto
        {
            Id = entidad.Id,
            Codigo = entidad.Codigo,
            Nombre = entidad.Nombre,
            Descripcion = entidad.Descripcion ?? string.Empty,
            Activo = entidad.Activo
        };
    }
}
