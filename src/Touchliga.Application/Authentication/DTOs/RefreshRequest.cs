namespace Touchliga.Application.Authentication.DTOs;

public sealed class RefreshRequest
{
    public string RefreshToken { get; init; } = string.Empty;
}
