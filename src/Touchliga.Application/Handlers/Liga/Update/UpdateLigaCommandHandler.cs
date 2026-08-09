using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Commands.Liga.Update;
using Touchliga.Application.Common.Interfaces;

namespace Touchliga.Application.Handlers.Liga.Update;

public sealed class UpdateLigaCommandHandler : IRequestHandler<UpdateLigaCommand, long>
{
    private readonly ILigaRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public UpdateLigaCommandHandler(
        ILigaRepository repository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<long> Handle(
        UpdateLigaCommand request,
        CancellationToken cancellationToken)
    {
        var liga = await _repository.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Liga");

        liga.ActualizarDatos(
            request.Nombre,
            request.Descripcion,
            request.Activo,
            _currentUser.UserId);

        _repository.Actualizar(liga);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return liga.Id;
    }
}
