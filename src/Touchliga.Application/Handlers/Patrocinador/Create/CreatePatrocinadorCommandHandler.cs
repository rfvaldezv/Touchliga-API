using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.Commands.Patrocinador.Create;
using Touchliga.Application.Common.Interfaces;
using DomainEntity = Touchliga.Domain.Entities.Patrocinador;

namespace Touchliga.Application.Handlers.Patrocinador.Create;

public sealed class CreatePatrocinadorCommandHandler : IRequestHandler<CreatePatrocinadorCommand, long>
{
    private readonly IPatrocinadorRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public CreatePatrocinadorCommandHandler(
        IPatrocinadorRepository repository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<long> Handle(CreatePatrocinadorCommand request, CancellationToken cancellationToken)
    {
        var patrocinador = DomainEntity.Crear(
            request.Codigo,
            request.Nombre,
            request.Descripcion,
            request.ImagenUrl,
            request.EnlaceUrl,
            request.Orden,
            request.Activo,
            _currentUser.UserId);

        await _repository.AgregarAsync(patrocinador, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return patrocinador.Id;
    }
}
