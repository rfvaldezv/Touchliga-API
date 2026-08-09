using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Commands.Patrocinador.Delete;

namespace Touchliga.Application.Handlers.Patrocinador.Delete;

public sealed class DeletePatrocinadorCommandHandler : IRequestHandler<DeletePatrocinadorCommand, Unit>
{
    private readonly IPatrocinadorRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeletePatrocinadorCommandHandler(IPatrocinadorRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeletePatrocinadorCommand request, CancellationToken cancellationToken)
    {
        var patrocinador = await _repository.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Patrocinador");

        _repository.Eliminar(patrocinador);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
