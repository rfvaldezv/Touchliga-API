using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.DTOs;
using Touchliga.Application.Queries.Patrocinador.Get;

namespace Touchliga.Application.Handlers.Patrocinador.Get;

public sealed class GetPatrocinadorQueryHandler : IRequestHandler<GetPatrocinadorQuery, PatrocinadorDto>
{
    private readonly IPatrocinadorRepository _repository;

    public GetPatrocinadorQueryHandler(IPatrocinadorRepository repository)
    {
        _repository = repository;
    }

    public async Task<PatrocinadorDto> Handle(GetPatrocinadorQuery request, CancellationToken cancellationToken)
    {
        var p = await _repository.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Patrocinador");

        return new PatrocinadorDto
        {
            Id = p.Id,
            Codigo = p.Codigo,
            Nombre = p.Nombre,
            Descripcion = p.Descripcion ?? string.Empty,
            ImagenUrl = p.ImagenUrl,
            EnlaceUrl = p.EnlaceUrl,
            Orden = p.Orden,
            Activo = p.Activo
        };
    }
}
