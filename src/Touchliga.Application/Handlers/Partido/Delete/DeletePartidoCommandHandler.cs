using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Commands.Partido.Delete;

namespace Touchliga.Application.Handlers.Partido.Delete;

public sealed class DeletePartidoCommandHandler : IRequestHandler<DeletePartidoCommand, Unit>
{
    private readonly IPartidoRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeletePartidoCommandHandler(IPartidoRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeletePartidoCommand request, CancellationToken cancellationToken)
    {
        var partido = await _repository.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Partido");

        _repository.Eliminar(partido);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
