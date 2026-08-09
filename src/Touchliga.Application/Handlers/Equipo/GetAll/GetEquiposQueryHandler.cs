using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.DTOs;
using Touchliga.Application.Queries.Equipo.GetAll;

namespace Touchliga.Application.Handlers.Equipo.GetAll;

public sealed class GetEquiposQueryHandler : IRequestHandler<GetEquiposQuery, IReadOnlyList<EquipoDto>>
{
    private readonly IEquipoRepository _repository;

    public GetEquiposQueryHandler(
        IEquipoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<EquipoDto>> Handle(
        GetEquiposQuery request,
        CancellationToken cancellationToken)
    {
        var entidades = await _repository.ObtenerTodosAsync(cancellationToken);

        return entidades
            .Select(entidad => new EquipoDto
            {
                Id = entidad.Id,
                Codigo = entidad.Codigo,
                Nombre = entidad.Nombre,
                Descripcion = entidad.Descripcion ?? string.Empty,
                Activo = entidad.Activo,
                EscudoUrl = entidad.EscudoUrl,
                Apodo = entidad.Apodo
            })
            .ToList();
    }
}
