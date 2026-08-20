using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.Communication.DTOs;
using Touchliga.Application.Communication.Queries.GetMisContactos;
using Touchliga.Application.Common.Interfaces;

namespace Touchliga.Application.Handlers.Communication.GetMisContactos;

public sealed class GetMisContactosQueryHandler
    : IRequestHandler<GetMisContactosQuery, IReadOnlyList<ContactoDto>>
{
    private readonly IMensajeRepository _mensajes;
    private readonly IUsuarioRepository _usuarios;
    private readonly ICurrentUserService _currentUser;

    public GetMisContactosQueryHandler(
        IMensajeRepository mensajes,
        IUsuarioRepository usuarios,
        ICurrentUserService currentUser)
    {
        _mensajes = mensajes;
        _usuarios = usuarios;
        _currentUser = currentUser;
    }

    public async Task<IReadOnlyList<ContactoDto>> Handle(
        GetMisContactosQuery request,
        CancellationToken cancellationToken)
    {
        var ultimos = await _mensajes.ObtenerUltimosPorContactoAsync(_currentUser.UserId, cancellationToken);
        var usuarios = await _usuarios.ObtenerTodosAsync();
        var usuariosPorId = usuarios.ToDictionary(u => u.Id);

        return ultimos.Select(m =>
        {
            var contactoId = m.RemitenteId == _currentUser.UserId ? m.DestinatarioId : m.RemitenteId;
            var contacto = usuariosPorId.GetValueOrDefault(contactoId);

            return new ContactoDto
            {
                UsuarioId = contactoId,
                Nombre = contacto != null ? $"{contacto.Nombre} {contacto.Apellidos}" : "Usuario",
                Telefono = contacto?.Telefono,
                UltimoMensaje = string.IsNullOrWhiteSpace(m.Contenido) ? "📷 Imagen" : m.Contenido,
                FechaUltimoMensaje = m.FechaEnvio,
                TieneNoLeidos = !m.Leido && m.DestinatarioId == _currentUser.UserId,
                Roles = contacto?.Roles.Select(r => r.Rol.Nombre).ToList() ?? new()
            };
        })
        .OrderByDescending(c => c.FechaUltimoMensaje)
        .ToList();
    }
}
