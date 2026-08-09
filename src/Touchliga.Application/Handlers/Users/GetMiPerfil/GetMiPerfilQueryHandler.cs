using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Users.DTOs;
using Touchliga.Application.Users.Queries.GetMiPerfil;
using Touchliga.Application.Common.Interfaces;

namespace Touchliga.Application.Handlers.Users.GetMiPerfil;

public sealed class GetMiPerfilQueryHandler : IRequestHandler<GetMiPerfilQuery, UsuarioAdminDto>
{
    private readonly IUsuarioRepository _usuarios;
    private readonly IEquipoRepository _equipos;
    private readonly ICurrentUserService _currentUser;

    public GetMiPerfilQueryHandler(
        IUsuarioRepository usuarios,
        IEquipoRepository equipos,
        ICurrentUserService currentUser)
    {
        _usuarios = usuarios;
        _equipos = equipos;
        _currentUser = currentUser;
    }

    public async Task<UsuarioAdminDto> Handle(GetMiPerfilQuery request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarios.ObtenerPorIdAsync(_currentUser.UserId)
            ?? throw new EntityNotFoundException("Usuario");

        string? equipoNombre = null;

        if (usuario.EquipoFavoritoId.HasValue)
        {
            var equipo = await _equipos.ObtenerPorIdAsync(usuario.EquipoFavoritoId.Value, cancellationToken);
            equipoNombre = equipo?.Nombre;
        }

        return new UsuarioAdminDto
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Apellidos = usuario.Apellidos,
            Telefono = usuario.Telefono,
            Correo = usuario.Correo.Value,
            Activo = usuario.Activo,
            Sexo = usuario.Sexo,
            FechaNacimiento = usuario.FechaNacimiento,
            Nickname = usuario.Nickname,
            EquipoFavoritoId = usuario.EquipoFavoritoId,
            EquipoFavoritoNombre = equipoNombre,
            FotoUrl = usuario.FotoUrl,
            Roles = usuario.Roles.Select(r => r.Rol.Nombre).ToList(),
        };
    }
}
