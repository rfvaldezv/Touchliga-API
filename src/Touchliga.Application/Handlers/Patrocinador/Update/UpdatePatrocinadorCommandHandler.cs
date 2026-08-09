using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Commands.Patrocinador.Update;
using Touchliga.Application.Common.Interfaces;

namespace Touchliga.Application.Handlers.Patrocinador.Update;

public sealed class UpdatePatrocinadorCommandHandler : IRequestHandler<UpdatePatrocinadorCommand, long>
{
    private readonly IPatrocinadorRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public UpdatePatrocinadorCommandHandler(
        IPatrocinadorRepository repository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<long> Handle(UpdatePatrocinadorCommand request, CancellationToken cancellationToken)
    {
        var patrocinador = await _repository.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Patrocinador");

        patrocinador.ActualizarDatos(
            request.Nombre,
            request.Descripcion,
            request.ImagenUrl,
            request.EnlaceUrl,
            request.Orden,
            request.Activo,
            _currentUser.UserId);

        _repository.Actualizar(patrocinador);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return patrocinador.Id;
    }
}
