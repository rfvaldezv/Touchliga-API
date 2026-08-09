using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.DTOs;
using Touchliga.Application.Queries.Cancha.Get;

namespace Touchliga.Application.Handlers.Cancha.Get;

public sealed class GetCanchaQueryHandler : IRequestHandler<GetCanchaQuery, CanchaDto>
{
    private readonly ICanchaRepository _repository;

    public GetCanchaQueryHandler(
        ICanchaRepository repository)
    {
        _repository = repository;
    }

    public async Task<CanchaDto> Handle(
        GetCanchaQuery request,
        CancellationToken cancellationToken)
    {
        var entidad = await _repository.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Cancha");

        return new CanchaDto
        {
            Id = entidad.Id,
            Codigo = entidad.Codigo,
            Nombre = entidad.Nombre,
            Descripcion = entidad.Descripcion ?? string.Empty,
            Activo = entidad.Activo
        };
    }
}
