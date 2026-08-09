using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.Commands.Categoria.Create;
using Touchliga.Application.Common.Interfaces;
using DomainEntity = Touchliga.Domain.Entities.Categoria;

namespace Touchliga.Application.Handlers.Categoria.Create;

public sealed class CreateCategoriaCommandHandler : IRequestHandler<CreateCategoriaCommand, long>
{
    private readonly ICategoriaRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public CreateCategoriaCommandHandler(
        ICategoriaRepository repository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<long> Handle(
        CreateCategoriaCommand request,
        CancellationToken cancellationToken)
    {
        var entidad = DomainEntity.Crear(
            request.Codigo,
            request.Nombre,
            request.Descripcion,
            request.Activo,
            _currentUser.UserId);

        await _repository.AgregarAsync(entidad, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entidad.Id;
    }
}
