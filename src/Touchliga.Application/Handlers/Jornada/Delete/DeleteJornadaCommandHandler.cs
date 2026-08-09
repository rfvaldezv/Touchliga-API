using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Commands.Jornada.Delete;

namespace Touchliga.Application.Handlers.Jornada.Delete;

public sealed class DeleteJornadaCommandHandler : IRequestHandler<DeleteJornadaCommand, Unit>
{
    private readonly IJornadaRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteJornadaCommandHandler(
        IJornadaRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(
        DeleteJornadaCommand request,
        CancellationToken cancellationToken)
    {
        var entidad = await _repository.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Jornada");

        _repository.Eliminar(entidad);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
