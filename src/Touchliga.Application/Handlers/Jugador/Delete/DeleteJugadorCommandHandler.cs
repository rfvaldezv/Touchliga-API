using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Commands.Jugador.Delete;

namespace Touchliga.Application.Handlers.Jugador.Delete;

public sealed class DeleteJugadorCommandHandler : IRequestHandler<DeleteJugadorCommand, Unit>
{
    private readonly IJugadorRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteJugadorCommandHandler(
        IJugadorRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(
        DeleteJugadorCommand request,
        CancellationToken cancellationToken)
    {
        var entidad = await _repository.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Jugador");

        _repository.Eliminar(entidad);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
