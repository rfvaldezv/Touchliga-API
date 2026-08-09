using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Commands.Estado.Delete;

namespace Touchliga.Application.Handlers.Estado.Delete;

public sealed class DeleteEstadoCommandHandler : IRequestHandler<DeleteEstadoCommand, Unit>
{
    private readonly IEstadoRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteEstadoCommandHandler(
        IEstadoRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(
        DeleteEstadoCommand request,
        CancellationToken cancellationToken)
    {
        var entidad = await _repository.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Estado");

        _repository.Eliminar(entidad);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
