using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Commands.Estado.Update;
using Touchliga.Application.Common.Interfaces;

namespace Touchliga.Application.Handlers.Estado.Update;

public sealed class UpdateEstadoCommandHandler : IRequestHandler<UpdateEstadoCommand, long>
{
    private readonly IEstadoRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public UpdateEstadoCommandHandler(
        IEstadoRepository repository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<long> Handle(
        UpdateEstadoCommand request,
        CancellationToken cancellationToken)
    {
        var entidad = await _repository.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Estado");

        entidad.ActualizarDatos(
            request.Nombre,
            request.Descripcion,
            request.PaisId,
            request.Activo,
            _currentUser.UserId);

        _repository.Actualizar(entidad);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entidad.Id;
    }
}
