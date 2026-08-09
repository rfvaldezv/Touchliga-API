using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Commands.Ciudad.Delete;

namespace Touchliga.Application.Handlers.Ciudad.Delete;

public sealed class DeleteCiudadCommandHandler : IRequestHandler<DeleteCiudadCommand, Unit>
{
    private readonly ICiudadRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCiudadCommandHandler(
        ICiudadRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(
        DeleteCiudadCommand request,
        CancellationToken cancellationToken)
    {
        var entidad = await _repository.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Ciudad");

        _repository.Eliminar(entidad);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
