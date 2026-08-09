using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Commands.Pais.Update;
using Touchliga.Application.Common.Interfaces;

namespace Touchliga.Application.Handlers.Pais.Update;

public sealed class UpdatePaisCommandHandler : IRequestHandler<UpdatePaisCommand, long>
{
    private readonly IPaisRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public UpdatePaisCommandHandler(
        IPaisRepository repository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<long> Handle(
        UpdatePaisCommand request,
        CancellationToken cancellationToken)
    {
        var entidad = await _repository.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Pais");

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
