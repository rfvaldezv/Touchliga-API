using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.DTOs;
using Touchliga.Application.Queries.Equipo.Get;

namespace Touchliga.Application.Handlers.Equipo.Get;

public sealed class GetEquipoQueryHandler : IRequestHandler<GetEquipoQuery, EquipoDto>
{
    private readonly IEquipoRepository _repository;

    public GetEquipoQueryHandler(
        IEquipoRepository repository)
    {
        _repository = repository;
    }

    public async Task<EquipoDto> Handle(
        GetEquipoQuery request,
        CancellationToken cancellationToken)
    {
        var entidad = await _repository.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Equipo");

        return new EquipoDto
        {
            Id = entidad.Id,
            Codigo = entidad.Codigo,
            Nombre = entidad.Nombre,
            Descripcion = entidad.Descripcion ?? string.Empty,
            Activo = entidad.Activo,
            EscudoUrl = entidad.EscudoUrl,
            Apodo = entidad.Apodo
        };
    }
}
