using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.DTOs;
using Touchliga.Application.Queries.Categoria.GetAll;

namespace Touchliga.Application.Handlers.Categoria.GetAll;

public sealed class GetCategoriasQueryHandler : IRequestHandler<GetCategoriasQuery, IReadOnlyList<CategoriaDto>>
{
    private readonly ICategoriaRepository _repository;

    public GetCategoriasQueryHandler(
        ICategoriaRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<CategoriaDto>> Handle(
        GetCategoriasQuery request,
        CancellationToken cancellationToken)
    {
        var entidades = await _repository.ObtenerTodosAsync(cancellationToken);

        return entidades
            .Select(entidad => new CategoriaDto
            {
                Id = entidad.Id,
                Codigo = entidad.Codigo,
                Nombre = entidad.Nombre,
                Descripcion = entidad.Descripcion ?? string.Empty,
                Activo = entidad.Activo
            })
            .ToList();
    }
}
