using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Commands.Categoria.Update;
using Touchliga.Application.Common.Interfaces;

namespace Touchliga.Application.Handlers.Categoria.Update;

public sealed class UpdateCategoriaCommandHandler : IRequestHandler<UpdateCategoriaCommand, long>
{
    private readonly ICategoriaRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public UpdateCategoriaCommandHandler(
        ICategoriaRepository repository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<long> Handle(
        UpdateCategoriaCommand request,
        CancellationToken cancellationToken)
    {
        var entidad = await _repository.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Categoria");

        entidad.ActualizarDatos(
            request.Nombre,
            request.Descripcion,
            request.Activo,
            _currentUser.UserId);

        _repository.Actualizar(entidad);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entidad.Id;
    }
}
