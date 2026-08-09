using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.Commands.Equipo.Create;
using Touchliga.Application.Common.Interfaces;
using DomainEntity = Touchliga.Domain.Entities.Equipo;

namespace Touchliga.Application.Handlers.Equipo.Create;

public sealed class CreateEquipoCommandHandler : IRequestHandler<CreateEquipoCommand, long>
{
    private readonly IEquipoRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public CreateEquipoCommandHandler(
        IEquipoRepository repository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<long> Handle(
        CreateEquipoCommand request,
        CancellationToken cancellationToken)
    {
        var entidad = DomainEntity.Crear(
            request.Codigo,
            request.Nombre,
            request.Descripcion,
            request.EscudoUrl,
            request.Apodo,
            request.Activo,
            _currentUser.UserId);

        await _repository.AgregarAsync(entidad, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entidad.Id;
    }
}
