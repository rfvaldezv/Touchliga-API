using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Communication.Commands.EliminarAnuncio;

namespace Touchliga.Application.Handlers.Communication.EliminarAnuncio;

public sealed class EliminarAnuncioCommandHandler : IRequestHandler<EliminarAnuncioCommand, Unit>
{
    private readonly IAnuncioRepository _anuncios;
    private readonly IUnitOfWork _unitOfWork;

    public EliminarAnuncioCommandHandler(
        IAnuncioRepository anuncios,
        IUnitOfWork unitOfWork)
    {
        _anuncios = anuncios;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(EliminarAnuncioCommand request, CancellationToken cancellationToken)
    {
        var anuncio = await _anuncios.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Anuncio");

        _anuncios.Eliminar(anuncio);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
