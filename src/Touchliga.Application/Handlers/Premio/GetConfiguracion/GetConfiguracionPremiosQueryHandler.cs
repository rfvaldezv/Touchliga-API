using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.DTOs;
using Touchliga.Application.Queries.Premio.GetConfiguracion;

namespace Touchliga.Application.Handlers.Premio.GetConfiguracion;

public sealed class GetConfiguracionPremiosQueryHandler
    : IRequestHandler<GetConfiguracionPremiosQuery, IReadOnlyList<ConfiguracionPremioDto>>
{
    private readonly IConfiguracionPremioRepository _premios;

    public GetConfiguracionPremiosQueryHandler(IConfiguracionPremioRepository premios)
    {
        _premios = premios;
    }

    public async Task<IReadOnlyList<ConfiguracionPremioDto>> Handle(
        GetConfiguracionPremiosQuery request,
        CancellationToken cancellationToken)
    {
        var premios = await _premios.ObtenerPorTemporadaYAmbitoAsync(
            request.TemporadaId, request.Ambito, cancellationToken);

        return premios.Select(p => new ConfiguracionPremioDto
        {
            Id = p.Id,
            Posicion = p.Posicion,
            TipoPremio = p.TipoPremio,
            Monto = p.Monto,
            Descripcion = p.Descripcion,
        }).ToList();
    }
}
