using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.DTOs;
using Touchliga.Application.Queries.Estandares.GetTablaPosiciones;

namespace Touchliga.Application.Handlers.Estandares.GetTablaPosiciones;

public sealed class GetTablaPosicionesQueryHandler
    : IRequestHandler<GetTablaPosicionesQuery, IReadOnlyList<PosicionDto>>
{
    private readonly IPosicionesRepository _repository;

    public GetTablaPosicionesQueryHandler(IPosicionesRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<PosicionDto>> Handle(
        GetTablaPosicionesQuery request,
        CancellationToken cancellationToken)
    {
        var posiciones = await _repository.ObtenerTablaPosicionesAsync(request.TemporadaId, cancellationToken);

        return posiciones
            .Select(p => new PosicionDto
            {
                UsuarioId = p.UsuarioId,
                Nombre = p.Nombre,
                Puntos = p.Puntos,
                Aciertos = p.Aciertos,
                Pronosticos = p.Pronosticos
            })
            .ToList();
    }
}
