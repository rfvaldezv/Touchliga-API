using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Entities;
using Touchliga.Application.Communication.Commands.CrearAnuncio;
using Touchliga.Application.Common.Interfaces;

namespace Touchliga.Application.Handlers.Communication.CrearAnuncio;

public sealed class CrearAnuncioCommandHandler : IRequestHandler<CrearAnuncioCommand, long>
{
    private readonly IAnuncioRepository _anuncios;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IPushNotificationService _push;

    public CrearAnuncioCommandHandler(
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

    public async Task<long> Handle(CrearAnuncioCommand request, CancellationToken cancellationToken)
    {
        var anuncio = Anuncio.Crear(request.Titulo, request.Contenido, _currentUser.UserId, request.ImagenUrl);

        await _anuncios.AgregarAsync(anuncio, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _push.EnviarATodosAsync(
            $"📢 {request.Titulo}",
            request.Contenido,
            cancellationToken);

        return anuncio.Id;
    }
}
