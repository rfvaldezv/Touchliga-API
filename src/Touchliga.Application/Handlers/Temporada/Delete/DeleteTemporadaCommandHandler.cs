using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Commands.Temporada.Delete;

namespace Touchliga.Application.Handlers.Temporada.Delete;

public sealed class DeleteTemporadaCommandHandler : IRequestHandler<DeleteTemporadaCommand, Unit>
{
    private readonly ITemporadaRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTemporadaCommandHandler(
        ITemporadaRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(
        DeleteTemporadaCommand request,
        CancellationToken cancellationToken)
    {
        var entidad = await _repository.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Temporada");

        _repository.Eliminar(entidad);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
