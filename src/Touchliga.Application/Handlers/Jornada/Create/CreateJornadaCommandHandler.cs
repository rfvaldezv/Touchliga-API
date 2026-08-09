using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.Commands.Jornada.Create;
using Touchliga.Application.Common.Interfaces;
using DomainEntity = Touchliga.Domain.Entities.Jornada;

namespace Touchliga.Application.Handlers.Jornada.Create;

public sealed class CreateJornadaCommandHandler : IRequestHandler<CreateJornadaCommand, long>
{
    private readonly IJornadaRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public CreateJornadaCommandHandler(
        IJornadaRepository repository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<long> Handle(CreateJornadaCommand request, CancellationToken cancellationToken)
    {
        var jornada = DomainEntity.Crear(
            request.TemporadaId,
            request.Codigo,
            request.Nombre,
            request.Descripcion,
            request.Numero,
            request.FechaCierre,
            request.Activo,
            _currentUser.UserId);

        await _repository.AgregarAsync(jornada, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return jornada.Id;
    }
}
