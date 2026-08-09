using Touchliga.Application.Authentication.DTOs;
using Touchliga.Application.Authentication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Touchliga.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _authenticationService.LoginAsync(
            request.Correo,
            request.Password,
            cancellationToken);

        return Ok(response);
    }

    /// <summary>
    /// Renueva la sesión usando el refresh token — el cliente lo llama
    /// automáticamente cuando el access token expira (o está por
    /// expirar), para no forzar al usuario a volver a loguearse cada
    /// hora. El refresh token usado se revoca y se emite uno nuevo.
    /// </summary>
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh(
        [FromBody] RefreshRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _authenticationService.RefreshAsync(
            request.RefreshToken,
            cancellationToken);

        return Ok(response);
    }

    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Logout(
        [FromBody] LogoutRequest request,
        CancellationToken cancellationToken)
    {
        await _authenticationService.LogoutAsync(request.RefreshToken, cancellationToken);

        return NoContent();
    }
}
