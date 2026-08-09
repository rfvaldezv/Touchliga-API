using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.Commands.Ciudad.Create;
using Touchliga.Application.Common.Interfaces;
using DomainEntity = Touchliga.Domain.Entities.Ciudad;

namespace Touchliga.Application.Handlers.Ciudad.Create;

public sealed class CreateCiudadCommandHandler : IRequestHandler<CreateCiudadCommand, long>
{
    private readonly ICiudadRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public CreateCiudadCommandHandler(
        ICiudadRepository repository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<long> Handle(
        CreateCiudadCommand request,
        CancellationToken cancellationToken)
    {
        var entidad = DomainEntity.Crear(
            request.Codigo,
            request.Nombre,
            request.Descripcion,
            request.PaisId,
            request.EstadoId,
            request.Activo,
            _currentUser.UserId);

        await _repository.AgregarAsync(entidad, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entidad.Id;
    }
}
