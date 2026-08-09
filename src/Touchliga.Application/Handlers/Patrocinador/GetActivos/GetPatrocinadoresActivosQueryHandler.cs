using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.DTOs;
using Touchliga.Application.Queries.Patrocinador.GetActivos;

namespace Touchliga.Application.Handlers.Patrocinador.GetActivos;

public sealed class GetPatrocinadoresActivosQueryHandler
    : IRequestHandler<GetPatrocinadoresActivosQuery, IReadOnlyList<PatrocinadorDto>>
{
    private readonly IPatrocinadorRepository _repository;

    public GetPatrocinadoresActivosQueryHandler(IPatrocinadorRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<PatrocinadorDto>> Handle(
        GetPatrocinadoresActivosQuery request, CancellationToken cancellationToken)
    {
        var patrocinadores = await _repository.ObtenerTodosAsync(cancellationToken);

        return patrocinadores
            .Where(p => p.Activo)
            .OrderBy(p => p.Orden)
            .Select(p => new PatrocinadorDto
        {
            Id = p.Id,
            Codigo = p.Codigo,
            Nombre = p.Nombre,
            Descripcion = p.Descripcion ?? string.Empty,
            ImagenUrl = p.ImagenUrl,
            EnlaceUrl = p.EnlaceUrl,
            Orden = p.Orden,
            Activo = p.Activo
        })
            .ToList();
    }
}
