using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Commands.Equipo.Update;
using Touchliga.Application.Common.Interfaces;

namespace Touchliga.Application.Handlers.Equipo.Update;

public sealed class UpdateEquipoCommandHandler : IRequestHandler<UpdateEquipoCommand, long>
{
    private readonly IEquipoRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public UpdateEquipoCommandHandler(
        IEquipoRepository repository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<long> Handle(
        UpdateEquipoCommand request,
        CancellationToken cancellationToken)
    {
        var entidad = await _repository.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Equipo");

        entidad.ActualizarDatos(
            request.Nombre,
            request.Descripcion,
            request.EscudoUrl,
            request.Apodo,
            request.Activo,
            _currentUser.UserId);

        _repository.Actualizar(entidad);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entidad.Id;
    }
}
