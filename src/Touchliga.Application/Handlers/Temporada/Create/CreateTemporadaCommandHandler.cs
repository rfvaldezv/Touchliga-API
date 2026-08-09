using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.Commands.Temporada.Create;
using Touchliga.Application.Common.Interfaces;
using DomainEntity = Touchliga.Domain.Entities.Temporada;

namespace Touchliga.Application.Handlers.Temporada.Create;

public sealed class CreateTemporadaCommandHandler : IRequestHandler<CreateTemporadaCommand, long>
{
    private readonly ITemporadaRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public CreateTemporadaCommandHandler(
        ITemporadaRepository repository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<long> Handle(CreateTemporadaCommand request, CancellationToken cancellationToken)
    {
        var temporada = DomainEntity.Crear(
            request.LigaId,
            request.Codigo,
            request.Nombre,
            request.Descripcion,
            request.FechaInicio,
            request.FechaFin,
            request.Cuota,
            request.Activo,
            _currentUser.UserId);

        await _repository.AgregarAsync(temporada, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return temporada.Id;
    }
}
