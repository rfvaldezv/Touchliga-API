using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Commands.Cancha.Delete;

namespace Touchliga.Application.Handlers.Cancha.Delete;

public sealed class DeleteCanchaCommandHandler : IRequestHandler<DeleteCanchaCommand, Unit>
{
    private readonly ICanchaRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCanchaCommandHandler(
        ICanchaRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(
        DeleteCanchaCommand request,
        CancellationToken cancellationToken)
    {
        var entidad = await _repository.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Cancha");

        _repository.Eliminar(entidad);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
