using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.Commands.PushToken.Registrar;
using Touchliga.Application.Common.Interfaces;
using DomainEntity = Touchliga.Domain.Entities.PushToken;

namespace Touchliga.Application.Handlers.PushToken.Registrar;

public sealed class RegistrarPushTokenCommandHandler : IRequestHandler<RegistrarPushTokenCommand, Unit>
{
    private readonly IPushTokenRepository _pushTokens;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public RegistrarPushTokenCommandHandler(
        IPushTokenRepository pushTokens,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _pushTokens = pushTokens;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(RegistrarPushTokenCommand request, CancellationToken cancellationToken)
    {
        var existente = await _pushTokens.ObtenerPorTokenAsync(request.Token, cancellationToken);

        if (existente != null)
        {
            // El mismo celular puede haber tenido otra sesión antes
            // (otro usuario, o el mismo). Se borra y se vuelve a
            // crear apuntando al usuario actual — más simple que
            // mutar la entidad para un caso tan puntual.
            _pushTokens.Eliminar(existente);
        }

        var nuevo = DomainEntity.Registrar(_currentUser.UserId, request.Token, request.Plataforma);
        await _pushTokens.AgregarAsync(nuevo, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
