using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Touchliga.Application.Authentication.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Touchliga.Infrastructure.Authentication.Jwt;

public sealed class JwtService : IJwtService
{
    private readonly JwtOptions _options;

    public JwtService(IOptions<JwtOptions> options)
    {
        _options = options.Value;

        if (string.IsNullOrWhiteSpace(_options.SecretKey))
            throw new InvalidOperationException(
                "La configuración Jwt:SecretKey es obligatoria.");
    }
    public string GenerateAccessToken(
        long usuarioId,
        string nombre,
        string correo,
        IEnumerable<string> roles)
    {
    var claims = new List<Claim>
        {
        new Claim(JwtRegisteredClaimNames.Sub, usuarioId.ToString()),
        new Claim(ClaimTypes.NameIdentifier, usuarioId.ToString()),

        new Claim(JwtRegisteredClaimNames.Email, correo),
        new Claim(ClaimTypes.Email, correo),

        new Claim(JwtRegisteredClaimNames.Name, nombre),
        new Claim(ClaimTypes.Name, nombre),

        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var rol in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, rol));
        }

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_options.SecretKey));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_options.ExpirationMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }

    public DateTime GetAccessTokenExpiration()
    {
        return DateTime.UtcNow.AddMinutes(_options.ExpirationMinutes);
    }
}
