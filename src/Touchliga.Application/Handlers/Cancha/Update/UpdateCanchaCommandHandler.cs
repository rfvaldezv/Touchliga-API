using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Commands.Cancha.Update;
using Touchliga.Application.Common.Interfaces;

namespace Touchliga.Application.Handlers.Cancha.Update;

public sealed class UpdateCanchaCommandHandler : IRequestHandler<UpdateCanchaCommand, long>
{
    private readonly ICanchaRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public UpdateCanchaCommandHandler(
        ICanchaRepository repository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<long> Handle(
        UpdateCanchaCommand request,
        CancellationToken cancellationToken)
    {
        var entidad = await _repository.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Cancha");

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
