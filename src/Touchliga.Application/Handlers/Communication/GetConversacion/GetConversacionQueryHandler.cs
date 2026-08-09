using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.Communication.DTOs;
using Touchliga.Application.Communication.Queries.GetConversacion;
using Touchliga.Application.Common.Interfaces;

namespace Touchliga.Application.Handlers.Communication.GetConversacion;

public sealed class GetConversacionQueryHandler
    : IRequestHandler<GetConversacionQuery, IReadOnlyList<MensajeDto>>
{
    private readonly IMensajeRepository _mensajes;
    private readonly ICurrentUserService _currentUser;

    public GetConversacionQueryHandler(IMensajeRepository mensajes, ICurrentUserService currentUser)
    {
        _mensajes = mensajes;
        _currentUser = currentUser;
    }

    public async Task<IReadOnlyList<MensajeDto>> Handle(
        GetConversacionQuery request,
        CancellationToken cancellationToken)
    {
        var mensajes = await _mensajes.ObtenerConversacionAsync(
            _currentUser.UserId, request.OtroUsuarioId, cancellationToken);

        return mensajes.Select(m => new MensajeDto
        {
            Id = m.Id,
            RemitenteId = m.RemitenteId,
            DestinatarioId = m.DestinatarioId,
            Contenido = m.Contenido,
            ImagenUrl = m.ImagenUrl,
            FechaEnvio = m.FechaEnvio,
            Leido = m.Leido,
            EsMio = m.RemitenteId == _currentUser.UserId
        }).ToList();
    }
}
