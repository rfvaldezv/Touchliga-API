using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.Common.Interfaces;
using Touchliga.Application.Communication.Queries.GetMensajesNoLeidos;

namespace Touchliga.Application.Handlers.Communication.GetMensajesNoLeidos;

public sealed class GetMensajesNoLeidosQueryHandler : IRequestHandler<GetMensajesNoLeidosQuery, int>
{
    private readonly IMensajeRepository _mensajes;
    private readonly ICurrentUserService _currentUser;

    public GetMensajesNoLeidosQueryHandler(IMensajeRepository mensajes, ICurrentUserService currentUser)
    {
        _mensajes = mensajes;
        _currentUser = currentUser;
    }

    public async Task<int> Handle(GetMensajesNoLeidosQuery request, CancellationToken cancellationToken)
    {
        return await _mensajes.ContarNoLeidosAsync(_currentUser.UserId, cancellationToken);
    }
}
