using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.Communication.DTOs;
using Touchliga.Application.Communication.Queries.GetTodosLosParticipantes;
using Touchliga.Application.Common.Interfaces;

namespace Touchliga.Application.Handlers.Communication.GetTodosLosParticipantes;

public sealed class GetTodosLosParticipantesQueryHandler
    : IRequestHandler<GetTodosLosParticipantesQuery, IReadOnlyList<ContactoDto>>
{
    private readonly IUsuarioRepository _usuarios;
    private readonly ICurrentUserService _currentUser;

    public GetTodosLosParticipantesQueryHandler(IUsuarioRepository usuarios, ICurrentUserService currentUser)
    {
        _usuarios = usuarios;
        _currentUser = currentUser;
    }

    public async Task<IReadOnlyList<ContactoDto>> Handle(
        GetTodosLosParticipantesQuery request,
        CancellationToken cancellationToken)
    {
        var usuarios = await _usuarios.ObtenerTodosAsync();

        return usuarios
            .Where(u => u.Id != _currentUser.UserId && u.Activo)
            .Select(u => new ContactoDto
            {
                UsuarioId = u.Id,
                Nombre = $"{u.Nombre} {u.Apellidos}",
                Telefono = u.Telefono,
                UltimoMensaje = string.Empty,
                FechaUltimoMensaje = DateTime.MinValue,
                TieneNoLeidos = false,
                Roles = u.Roles.Select(r => r.Rol.Nombre).ToList()
            })
            .OrderBy(c => c.Nombre)
            .ToList();
    }
}
