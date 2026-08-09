using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.DTOs;
using Touchliga.Application.Queries.Patrocinador.GetAll;

namespace Touchliga.Application.Handlers.Patrocinador.GetAll;

public sealed class GetPatrocinadoresQueryHandler
    : IRequestHandler<GetPatrocinadoresQuery, IReadOnlyList<PatrocinadorDto>>
{
    private readonly IPatrocinadorRepository _repository;

    public GetPatrocinadoresQueryHandler(IPatrocinadorRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<PatrocinadorDto>> Handle(
        GetPatrocinadoresQuery request, CancellationToken cancellationToken)
    {
        var patrocinadores = await _repository.ObtenerTodosAsync(cancellationToken);

        return patrocinadores
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
