using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Commands.Equipo.Delete;

namespace Touchliga.Application.Handlers.Equipo.Delete;

public sealed class DeleteEquipoCommandHandler : IRequestHandler<DeleteEquipoCommand, Unit>
{
    private readonly IEquipoRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteEquipoCommandHandler(
        IEquipoRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(
        DeleteEquipoCommand request,
        CancellationToken cancellationToken)
    {
        var entidad = await _repository.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Equipo");

        _repository.Eliminar(entidad);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
