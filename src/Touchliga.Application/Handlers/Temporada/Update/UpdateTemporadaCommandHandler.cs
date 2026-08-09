using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Commands.Temporada.Update;
using Touchliga.Application.Common.Interfaces;

namespace Touchliga.Application.Handlers.Temporada.Update;

public sealed class UpdateTemporadaCommandHandler : IRequestHandler<UpdateTemporadaCommand, long>
{
    private readonly ITemporadaRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public UpdateTemporadaCommandHandler(
        ITemporadaRepository repository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<long> Handle(UpdateTemporadaCommand request, CancellationToken cancellationToken)
    {
        var temporada = await _repository.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Temporada");

        temporada.ActualizarDatos(
            request.Nombre,
            request.Descripcion,
            request.FechaInicio,
            request.FechaFin,
            request.Cuota,
            request.Activo,
            _currentUser.UserId);

        _repository.Actualizar(temporada);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return temporada.Id;
    }
}
