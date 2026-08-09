using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.Commands.Estado.Create;
using Touchliga.Application.Common.Interfaces;
using DomainEntity = Touchliga.Domain.Entities.Estado;

namespace Touchliga.Application.Handlers.Estado.Create;

public sealed class CreateEstadoCommandHandler : IRequestHandler<CreateEstadoCommand, long>
{
    private readonly IEstadoRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public CreateEstadoCommandHandler(
        IEstadoRepository repository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<long> Handle(
        CreateEstadoCommand request,
        CancellationToken cancellationToken)
    {
        var entidad = DomainEntity.Crear(
            request.Codigo,
            request.Nombre,
            request.Descripcion,
            request.PaisId,
            request.Activo,
            _currentUser.UserId);

        await _repository.AgregarAsync(entidad, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entidad.Id;
    }
}
