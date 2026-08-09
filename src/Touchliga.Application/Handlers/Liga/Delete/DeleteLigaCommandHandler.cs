using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Commands.Liga.Delete;

namespace Touchliga.Application.Handlers.Liga.Delete;

public sealed class DeleteLigaCommandHandler : IRequestHandler<DeleteLigaCommand, Unit>
{
    private readonly ILigaRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteLigaCommandHandler(
        ILigaRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(
        DeleteLigaCommand request,
        CancellationToken cancellationToken)
    {
        var liga = await _repository.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Liga");

        _repository.Eliminar(liga);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
