using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.Commands.PushToken.Eliminar;

namespace Touchliga.Application.Handlers.PushToken.Eliminar;

public sealed class EliminarPushTokenCommandHandler : IRequestHandler<EliminarPushTokenCommand, Unit>
{
    private readonly IPushTokenRepository _pushTokens;
    private readonly IUnitOfWork _unitOfWork;

    public EliminarPushTokenCommandHandler(IPushTokenRepository pushTokens, IUnitOfWork unitOfWork)
    {
        _pushTokens = pushTokens;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(EliminarPushTokenCommand request, CancellationToken cancellationToken)
    {
        var existente = await _pushTokens.ObtenerPorTokenAsync(request.Token, cancellationToken);

        if (existente is null) return Unit.Value;

        _pushTokens.Eliminar(existente);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
