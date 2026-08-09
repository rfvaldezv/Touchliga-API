using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Commands.Jornada.Update;
using Touchliga.Application.Common.Interfaces;

namespace Touchliga.Application.Handlers.Jornada.Update;

public sealed class UpdateJornadaCommandHandler : IRequestHandler<UpdateJornadaCommand, long>
{
    private readonly IJornadaRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public UpdateJornadaCommandHandler(
        IJornadaRepository repository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<long> Handle(UpdateJornadaCommand request, CancellationToken cancellationToken)
    {
        var jornada = await _repository.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Jornada");

        jornada.ActualizarDatos(
            request.Nombre,
            request.Descripcion,
            request.Numero,
            request.FechaCierre,
            request.Activo,
            _currentUser.UserId);

        _repository.Actualizar(jornada);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return jornada.Id;
    }
}
