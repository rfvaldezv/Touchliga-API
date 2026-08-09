using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.DTOs;
using Touchliga.Application.Queries.Liga.Get;

namespace Touchliga.Application.Handlers.Liga.Get;

public sealed class GetLigaQueryHandler : IRequestHandler<GetLigaQuery, LigaDto>
{
    private readonly ILigaRepository _repository;

    public GetLigaQueryHandler(
        ILigaRepository repository)
    {
        _repository = repository;
    }

    public async Task<LigaDto> Handle(
        GetLigaQuery request,
        CancellationToken cancellationToken)
    {
        var liga = await _repository.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Liga");

        return new LigaDto
        {
            Id = liga.Id,
            Codigo = liga.Codigo,
            Nombre = liga.Nombre,
            Descripcion = liga.Descripcion ?? string.Empty,
            Activo = liga.Activo
        };
    }
}
