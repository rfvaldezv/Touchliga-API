using Touchliga.Application.Authentication.DTOs;
using Touchliga.Application.Authentication.Interfaces;
using Touchliga.Domain.Entities;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using MediatR;

namespace Touchliga.Application.Authentication.Commands.Login;

public sealed class LoginCommandHandler
    : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IUsuarioRepository _usuarios;

    private readonly IUsuarioRolRepository _usuarioRoles;

    private readonly IPasswordHasher _passwordHasher;

    private readonly IJwtService _jwt;

    private readonly ICredencialAlternaRepository _credencialesAlternas;

    public LoginCommandHandler(

        IUsuarioRepository usuarios,

        IUsuarioRolRepository usuarioRoles,

        IPasswordHasher passwordHasher,

        IJwtService jwt,

        ICredencialAlternaRepository credencialesAlternas)
    {
        _usuarios = usuarios;

        _usuarioRoles = usuarioRoles;

        _passwordHasher = passwordHasher;

        _jwt = jwt;

        _credencialesAlternas = credencialesAlternas;
    }

    public async Task<LoginResponse> Handle(

        LoginCommand request,

        CancellationToken cancellationToken)
    {
        var usuario =
            await _usuarios.ObtenerPorCorreoAsync(request.Correo);

        // Login normal con la cuenta principal -- este camino se
        // comporta EXACTAMENTE igual que antes, sin ningún cambio,
        // salvo que una cuenta marcada como "vinculada" NUNCA cuenta
        // como login principal válido, aunque el correo/contraseña
        // técnicamente coincidan -- su correo+contraseña originales
        // ahora solo funcionan a través de la credencial alterna que
        // apunta a la cuenta real donde juega.
        var esLoginPrincipalValido =
            usuario != null &&
            !usuario.EsCuentaVinculada &&
            _passwordHasher.Verify(request.Password, usuario.PasswordHash);

        if (!esLoginPrincipalValido)
        {
            // No coincidió como cuenta principal -- se revisa si el
            // correo/contraseña corresponden a una credencial alterna
            // (pareja/familiar que comparte la misma cuenta/puntos).
            var credencialAlterna =
                await _credencialesAlternas.ObtenerPorCorreoAsync(request.Correo, cancellationToken);

            var esCredencialAlternaValida =
                credencialAlterna != null &&
                _passwordHasher.Verify(request.Password, credencialAlterna.PasswordHash);

            if (!esCredencialAlternaValida)
                throw new DomainException("Usuario o contraseña incorrectos.");

            usuario = await _usuarios.ObtenerPorIdAsync(credencialAlterna!.UsuarioId)
                ?? throw new DomainException("Usuario o contraseña incorrectos.");
        }

        var roles = await _usuarioRoles.ObtenerRolesAsync(usuario!.Id);
        var nombresRoles = roles.Select(r => r.Rol.Nombre).ToList();

        var accessToken =
            _jwt.GenerateAccessToken(
                usuario.Id,
                usuario.Nombre,
                usuario.Correo,
                nombresRoles);

        var refreshToken =
            _jwt.GenerateRefreshToken();

        return new LoginResponse
        {
            UsuarioId = usuario.Id,

            Nombre = usuario.Nombre,

            Correo = usuario.Correo,

            AccessToken = accessToken,

            RefreshToken = refreshToken,

            Expira = _jwt.GetAccessTokenExpiration(),

            Roles = nombresRoles
        };
    }
}
