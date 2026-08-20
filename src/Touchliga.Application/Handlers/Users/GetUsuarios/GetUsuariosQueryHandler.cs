using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.Users.DTOs;
using Touchliga.Application.Users.Queries.GetUsuarios;

namespace Touchliga.Application.Handlers.Users.GetUsuarios;

public sealed class GetUsuariosQueryHandler
    : IRequestHandler<GetUsuariosQuery, IReadOnlyList<UsuarioAdminDto>>
{
    private readonly IUsuarioRepository _usuarios;
    private readonly ICiudadRepository _ciudades;
    private readonly IPaisRepository _paises;
    private readonly IEstadoRepository _estados;
    private readonly IEquipoRepository _equipos;
    private readonly ICredencialAlternaRepository _credencialesAlternas;

    public GetUsuariosQueryHandler(
        IUsuarioRepository usuarios,
        ICiudadRepository ciudades,
        IPaisRepository paises,
        IEstadoRepository estados,
        IEquipoRepository equipos,
        ICredencialAlternaRepository credencialesAlternas)
    {
        _usuarios = usuarios;
        _ciudades = ciudades;
        _paises = paises;
        _estados = estados;
        _equipos = equipos;
        _credencialesAlternas = credencialesAlternas;
    }

    public async Task<IReadOnlyList<UsuarioAdminDto>> Handle(
        GetUsuariosQuery request,
        CancellationToken cancellationToken)
    {
        var usuarios = await _usuarios.ObtenerTodosAsync();

        var ciudades = (await _ciudades.ObtenerTodosAsync(cancellationToken))
            .ToDictionary(c => c.Id, c => c.Nombre);
        var paises = (await _paises.ObtenerTodosAsync(cancellationToken))
            .ToDictionary(p => p.Id, p => p.Nombre);
        var estados = (await _estados.ObtenerTodosAsync(cancellationToken))
            .ToDictionary(e => e.Id, e => e.Nombre);
        var equipos = (await _equipos.ObtenerTodosAsync(cancellationToken))
            .ToDictionary(e => e.Id, e => e.Nombre);
        var nombresPorUsuarioId = usuarios.ToDictionary(u => u.Id, u => u.Nombre + " " + u.Apellidos);
        var credencialesAlternasPorUsuarioId = (await _credencialesAlternas.ObtenerTodasAsync(cancellationToken))
            .ToDictionary(c => c.UsuarioId, c => c.Correo.Value);

        return usuarios.Select(u => new UsuarioAdminDto
        {
            Id = u.Id,
            Nombre = u.Nombre,
            Apellidos = u.Apellidos,
            Telefono = u.Telefono,
            Correo = u.Correo.Value,
            Activo = u.Activo,
            Estatus = u.Estatus.ToString(),
            Sexo = u.Sexo,
            FechaNacimiento = u.FechaNacimiento,
            Nickname = u.Nickname,
            EquipoFavoritoId = u.EquipoFavoritoId,
            EquipoFavoritoNombre = u.EquipoFavoritoId.HasValue && equipos.TryGetValue(u.EquipoFavoritoId.Value, out var eq) ? eq : null,
            FotoUrl = u.FotoUrl,
            Roles = u.Roles.Select(r => r.Rol.Nombre).ToList(),
            InvitadoPorId = u.InvitadoPorId,
            InvitadoPorNombre = u.InvitadoPorId.HasValue && nombresPorUsuarioId.TryGetValue(u.InvitadoPorId.Value, out var inv)
                ? inv
                : null,
            ParejaId = u.ParejaId,
            ParejaNombre = u.ParejaId.HasValue && nombresPorUsuarioId.TryGetValue(u.ParejaId.Value, out var par)
                ? par
                : null,
            NombreEquipo = u.NombreEquipo,
            CorreoAlterna = credencialesAlternasPorUsuarioId.TryGetValue(u.Id, out var correoAlt) ? correoAlt : null,
            EsCuentaVinculada = u.EsCuentaVinculada,
            CiudadId = u.CiudadId,
            CiudadNombre = u.CiudadId.HasValue && ciudades.TryGetValue(u.CiudadId.Value, out var c) ? c : null,
            PaisId = u.PaisId,
            PaisNombre = u.PaisId.HasValue && paises.TryGetValue(u.PaisId.Value, out var p) ? p : null,
            EstadoId = u.EstadoId,
            EstadoNombre = u.EstadoId.HasValue && estados.TryGetValue(u.EstadoId.Value, out var e) ? e : null,
        }).ToList();
    }
}
