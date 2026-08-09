using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.DTOs;
using Touchliga.Application.Queries.Categoria.Get;

namespace Touchliga.Application.Handlers.Categoria.Get;

public sealed class GetCategoriaQueryHandler : IRequestHandler<GetCategoriaQuery, CategoriaDto>
{
    private readonly ICategoriaRepository _repository;

    public GetCategoriaQueryHandler(
        ICategoriaRepository repository)
    {
        _repository = repository;
    }

    public async Task<CategoriaDto> Handle(
        GetCategoriaQuery request,
        CancellationToken cancellationToken)
    {
        var entidad = await _repository.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Categoria");

        return new CategoriaDto
        {
            Id = entidad.Id,
            Codigo = entidad.Codigo,
            Nombre = entidad.Nombre,
            Descripcion = entidad.Descripcion ?? string.Empty,
            Activo = entidad.Activo
        };
    }
}
