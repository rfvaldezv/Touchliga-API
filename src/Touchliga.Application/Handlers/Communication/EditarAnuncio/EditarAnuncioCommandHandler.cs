using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Communication.Commands.EditarAnuncio;
using Touchliga.Application.Common.Interfaces;

namespace Touchliga.Application.Handlers.Communication.EditarAnuncio;

public sealed class EditarAnuncioCommandHandler : IRequestHandler<EditarAnuncioCommand, Unit>
{
    private readonly IAnuncioRepository _anuncios;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IPushNotificationService _push;

    public EditarAnuncioCommandHandler(
        IAnuncioRepository anuncios,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser,
        IPushNotificationService push)
    {
        _anuncios = anuncios;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _push = push;
    }

    public async Task<Unit> Handle(EditarAnuncioCommand request, CancellationToken cancellationToken)
    {
        var anuncio = await _anuncios.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Anuncio");

        anuncio.Editar(request.Titulo, request.Contenido, _currentUser.UserId, request.ImagenUrl);

        _anuncios.Actualizar(anuncio);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (request.ReenviarPush)
        {
            await _push.EnviarATodosAsync(
                $"📢 {request.Titulo} (actualizado)",
                request.Contenido,
                cancellationToken);
        }

        return Unit.Value;
    }
}
