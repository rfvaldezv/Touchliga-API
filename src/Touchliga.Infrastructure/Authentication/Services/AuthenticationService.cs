using Touchliga.Application.Authentication.DTOs;
using Touchliga.Application.Authentication.Interfaces;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Entities;

namespace Touchliga.Infrastructure.Authentication.Services;

public sealed class AuthenticationService : IAuthenticationService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ISesionRepository _sesionRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUsuarioRolRepository _usuarioRolRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICredencialAlternaRepository _credencialAlternaRepository;

    public AuthenticationService(
        IUsuarioRepository usuarioRepository,
        ISesionRepository sesionRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IUsuarioRolRepository usuarioRolRepository,
        IPasswordHasher passwordHasher,
        IJwtService jwtService,
        IUnitOfWork unitOfWork,
        ICredencialAlternaRepository credencialAlternaRepository)
    {
                _usuarioRepository = usuarioRepository;
                _sesionRepository = sesionRepository;
                _refreshTokenRepository = refreshTokenRepository;
                _usuarioRolRepository = usuarioRolRepository;
                _passwordHasher = passwordHasher;
                _jwtService = jwtService;
                _unitOfWork = unitOfWork;
                _credencialAlternaRepository = credencialAlternaRepository;
    }

public async Task<LoginResponse> LoginAsync(
    string correo,
    string password,
    CancellationToken cancellationToken = default)
{
    var usuario = await _usuarioRepository.ObtenerPorCorreoAsync(correo);

    // Login normal con la cuenta principal -- una cuenta marcada
    // como "vinculada" NUNCA cuenta como login principal válido,
    // aunque el correo/contraseña técnicamente coincidan -- su
    // correo+contraseña originales ahora solo funcionan a través de
    // la credencial alterna que apunta a la cuenta real donde juega.
    var esLoginPrincipalValido =
        usuario != null &&
        !usuario.EsCuentaVinculada &&
        _passwordHasher.Verify(password, usuario.PasswordHash);

    if (!esLoginPrincipalValido)
    {
        var credencialAlterna = await _credencialAlternaRepository.ObtenerPorCorreoAsync(correo, cancellationToken);

        var esCredencialAlternaValida =
            credencialAlterna != null &&
            _passwordHasher.Verify(password, credencialAlterna.PasswordHash);

        if (!esCredencialAlternaValida)
            throw new UnauthorizedAccessException("Usuario o contraseña incorrectos.");

        usuario = await _usuarioRepository.ObtenerPorIdAsync(credencialAlterna!.UsuarioId)
            ?? throw new UnauthorizedAccessException("Usuario o contraseña incorrectos.");
    }

    if (usuario is null)
        throw new UnauthorizedAccessException("Usuario o contraseña incorrectos.");

    var usuarioRoles = await _usuarioRolRepository.ObtenerRolesAsync(usuario.Id);
    var nombresRoles = usuarioRoles.Select(ur => ur.Rol.Nombre).ToList();

    // Si alguien está vinculado a esta cuenta, se muestra el nombre
    // combinado -- "Pedro y Ximena" -- en vez de solo el del titular.
    var nombreVinculado = await _credencialAlternaRepository.ObtenerNombreVinculadoAsync(usuario.Id, cancellationToken);
    var nombreParaMostrar = nombreVinculado == null
        ? usuario.Nombre
        : $"{usuario.Nombre} y {nombreVinculado}";

    var sesion = Sesion.Crear(
        usuario.Id,
        "127.0.0.1",
        "Swagger",
        "Windows",
        "Swagger",
        usuario.Id);

    await _sesionRepository.AgregarAsync(sesion);

    var accessToken = _jwtService.GenerateAccessToken(
        usuario.Id,
        nombreParaMostrar,
        usuario.Correo.Value,
        nombresRoles);

    var refreshToken = RefreshToken.Crear(
        usuario.Id,
        _jwtService.GenerateRefreshToken(),
        DateTime.UtcNow.AddDays(30),
        usuario.Id);

    await _refreshTokenRepository.AgregarAsync(refreshToken);

    await _unitOfWork.SaveChangesAsync(cancellationToken);

    return new LoginResponse
    {
        UsuarioId = usuario.Id,
        Nombre = nombreParaMostrar,
        Correo = usuario.Correo.Value,
        AccessToken = accessToken,
        RefreshToken = refreshToken.Token,
        Expira = _jwtService.GetAccessTokenExpiration(),
        Roles = nombresRoles
    };
}

    public async Task<LoginResponse> RefreshAsync(
        string refreshToken,
        CancellationToken cancellationToken = default)
{
        var tokenGuardado = await _refreshTokenRepository.ObtenerAsync(refreshToken);

        if (tokenGuardado is null || !tokenGuardado.EstaVigente())
            throw new UnauthorizedAccessException("Sesión expirada. Vuelve a iniciar sesión.");

        var usuario = await _usuarioRepository.ObtenerPorIdAsync(tokenGuardado.UsuarioId)
            ?? throw new UnauthorizedAccessException("Usuario no encontrado.");

        // Rotación: el refresh token usado se revoca y se emite uno nuevo,
        // así que un token robado solo sirve una vez.
        tokenGuardado.Revocar("Renovado", usuario.Id);
        await _refreshTokenRepository.ActualizarAsync(tokenGuardado);

        var usuarioRoles = await _usuarioRolRepository.ObtenerRolesAsync(usuario.Id);
        var nombresRoles = usuarioRoles.Select(ur => ur.Rol.Nombre).ToList();

        var nombreVinculado = await _credencialAlternaRepository.ObtenerNombreVinculadoAsync(usuario.Id, cancellationToken);
        var nombreParaMostrar = nombreVinculado == null
            ? usuario.Nombre
            : $"{usuario.Nombre} y {nombreVinculado}";

        var accessToken = _jwtService.GenerateAccessToken(
            usuario.Id,
            nombreParaMostrar,
            usuario.Correo.Value,
            nombresRoles);

        var nuevoRefreshToken = RefreshToken.Crear(
            usuario.Id,
            _jwtService.GenerateRefreshToken(),
            DateTime.UtcNow.AddDays(30),
            usuario.Id);

        await _refreshTokenRepository.AgregarAsync(nuevoRefreshToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new LoginResponse
        {
            UsuarioId = usuario.Id,
            Nombre = nombreParaMostrar,
            Correo = usuario.Correo.Value,
            AccessToken = accessToken,
            RefreshToken = nuevoRefreshToken.Token,
            Expira = _jwtService.GetAccessTokenExpiration(),
            Roles = nombresRoles
        };
}

    public async Task LogoutAsync(
        string refreshToken,
        CancellationToken cancellationToken = default)
{
        var tokenGuardado = await _refreshTokenRepository.ObtenerAsync(refreshToken);

        if (tokenGuardado is null || !tokenGuardado.EstaVigente())
            return; // Ya no está vigente o no existe: no hay nada que revocar.

        tokenGuardado.Revocar("Logout", tokenGuardado.UsuarioId);
        await _refreshTokenRepository.ActualizarAsync(tokenGuardado);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
}
}
