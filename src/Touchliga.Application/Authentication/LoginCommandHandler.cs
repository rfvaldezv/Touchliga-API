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

    public LoginCommandHandler(

        IUsuarioRepository usuarios,

        IUsuarioRolRepository usuarioRoles,

        IPasswordHasher passwordHasher,

        IJwtService jwt)
    {
        _usuarios = usuarios;

        _usuarioRoles = usuarioRoles;

        _passwordHasher = passwordHasher;

        _jwt = jwt;
    }

    public async Task<LoginResponse> Handle(

        LoginCommand request,

        CancellationToken cancellationToken)
    {
        var usuario =
            await _usuarios.ObtenerPorCorreoAsync(request.Correo);

        if (usuario == null)
            throw new DomainException(
                "Usuario o contraseña incorrectos.");

        if (!_passwordHasher.Verify(
                request.Password,
                usuario.PasswordHash))
        {
            throw new DomainException(
                "Usuario o contraseña incorrectos.");
        }

        var roles = await _usuarioRoles.ObtenerRolesAsync(usuario.Id);
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
