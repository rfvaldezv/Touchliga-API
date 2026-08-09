using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Commands.Categoria.Delete;

namespace Touchliga.Application.Handlers.Categoria.Delete;

public sealed class DeleteCategoriaCommandHandler : IRequestHandler<DeleteCategoriaCommand, Unit>
{
    private readonly ICategoriaRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCategoriaCommandHandler(
        ICategoriaRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(
        DeleteCategoriaCommand request,
        CancellationToken cancellationToken)
    {
        var entidad = await _repository.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Categoria");

        _repository.Eliminar(entidad);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
