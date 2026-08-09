using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.Commands.Pais.Create;
using Touchliga.Application.Common.Interfaces;
using DomainEntity = Touchliga.Domain.Entities.Pais;

namespace Touchliga.Application.Handlers.Pais.Create;

public sealed class CreatePaisCommandHandler : IRequestHandler<CreatePaisCommand, long>
{
    private readonly IPaisRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public CreatePaisCommandHandler(
        IPaisRepository repository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<long> Handle(
        CreatePaisCommand request,
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
