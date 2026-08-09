using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.DTOs;
using Touchliga.Application.Queries.Jornada.GetAll;
using Touchliga.Application.Handlers.Jornada.Get;

namespace Touchliga.Application.Handlers.Jornada.GetAll;

public sealed class GetJornadasQueryHandler : IRequestHandler<GetJornadasQuery, IReadOnlyList<JornadaDto>>
{
    private readonly IJornadaRepository _repository;

    public GetJornadasQueryHandler(IJornadaRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<JornadaDto>> Handle(GetJornadasQuery request, CancellationToken cancellationToken)
    {
        var jornadas = await _repository.ObtenerTodosAsync(cancellationToken);

        if (request.TemporadaId.HasValue)
        {
            jornadas = jornadas.Where(j => j.TemporadaId == request.TemporadaId.Value).ToList();
        }

        return jornadas.Select(GetJornadaQueryHandler.Map).ToList();
    }
}
