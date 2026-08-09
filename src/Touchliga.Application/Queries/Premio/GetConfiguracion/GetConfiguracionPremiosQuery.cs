using MediatR;
using Touchliga.Application.DTOs;

namespace Touchliga.Application.Queries.Premio.GetConfiguracion;

public sealed record GetConfiguracionPremiosQuery(long TemporadaId, string Ambito)
    : IRequest<IReadOnlyList<ConfiguracionPremioDto>>;
