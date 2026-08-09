using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.Communication.DTOs;
using Touchliga.Application.Communication.Queries.GetOrganizadores;

namespace Touchliga.Application.Handlers.Communication.GetOrganizadores;

public sealed class GetOrganizadoresQueryHandler
    : IRequestHandler<GetOrganizadoresQuery, IReadOnlyList<ContactoDto>>
{
    private readonly IUsuarioRepository _usuarios;

    public GetOrganizadoresQueryHandler(IUsuarioRepository usuarios)
    {
        _usuarios = usuarios;
    }

    public async Task<IReadOnlyList<ContactoDto>> Handle(
        GetOrganizadoresQuery request,
        CancellationToken cancellationToken)
    {
        var usuarios = await _usuarios.ObtenerTodosAsync();

        return usuarios
            .Where(u => u.TieneRol("Administrador") || u.TieneRol("Capturador"))
            .Select(u => new ContactoDto
            {
                UsuarioId = u.Id,
                Nombre = $"{u.Nombre} {u.Apellidos}",
                UltimoMensaje = string.Empty,
                FechaUltimoMensaje = DateTime.MinValue,
                TieneNoLeidos = false,
                Roles = u.Roles.Select(r => r.Rol.Nombre).ToList()
            })
            .ToList();
    }
}
