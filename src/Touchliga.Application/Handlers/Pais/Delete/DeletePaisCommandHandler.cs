using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Commands.Pais.Delete;

namespace Touchliga.Application.Handlers.Pais.Delete;

public sealed class DeletePaisCommandHandler : IRequestHandler<DeletePaisCommand, Unit>
{
    private readonly IPaisRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeletePaisCommandHandler(
        IPaisRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(
        DeletePaisCommand request,
        CancellationToken cancellationToken)
    {
        var entidad = await _repository.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Pais");

        _repository.Eliminar(entidad);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
