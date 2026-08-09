using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.DTOs;
using Touchliga.Application.Queries.Pronostico.GetMiosPorJornada;
using Touchliga.Application.Common.Interfaces;

namespace Touchliga.Application.Handlers.Pronostico.GetMiosPorJornada;

public sealed class GetMisPronosticosPorJornadaQueryHandler
    : IRequestHandler<GetMisPronosticosPorJornadaQuery, IReadOnlyList<PronosticoDto>>
{
    private readonly IPartidoRepository _partidos;
    private readonly IPronosticoRepository _pronosticos;
    private readonly ICurrentUserService _currentUser;

    public GetMisPronosticosPorJornadaQueryHandler(
        IPartidoRepository partidos,
        IPronosticoRepository pronosticos,
        ICurrentUserService currentUser)
    {
        _partidos = partidos;
        _pronosticos = pronosticos;
        _currentUser = currentUser;
    }

    public async Task<IReadOnlyList<PronosticoDto>> Handle(
        GetMisPronosticosPorJornadaQuery request,
        CancellationToken cancellationToken)
    {
        var partidos = await _partidos.ObtenerPorJornadaAsync(request.JornadaId, cancellationToken);
        var partidoIds = partidos.Select(p => p.Id).ToList();

        var pronosticos = await _pronosticos.ObtenerPorPartidoIdsAsync(partidoIds, cancellationToken);

        return pronosticos
            .Where(p => p.UsuarioId == _currentUser.UserId)
            .Select(p => new PronosticoDto
            {
                Id = p.Id,
                PartidoId = p.PartidoId,
                UsuarioId = p.UsuarioId,
                EquipoGanadorId = p.EquipoGanadorId,
                Puntos = p.Puntos,
                PuntosTotalesPredichos = p.PuntosTotalesPredichos,
                DiferenciaPuntosPredicha = p.DiferenciaPuntosPredicha,
                PuntosBono = p.PuntosBono
            })
            .ToList();
    }
}
