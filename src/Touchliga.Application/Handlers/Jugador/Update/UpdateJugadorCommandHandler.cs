using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Commands.Jugador.Update;
using Touchliga.Application.Common.Interfaces;

namespace Touchliga.Application.Handlers.Jugador.Update;

public sealed class UpdateJugadorCommandHandler : IRequestHandler<UpdateJugadorCommand, long>
{
    private readonly IJugadorRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public UpdateJugadorCommandHandler(
        IJugadorRepository repository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<long> Handle(
        UpdateJugadorCommand request,
        CancellationToken cancellationToken)
    {
        var entidad = await _repository.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Jugador");

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
