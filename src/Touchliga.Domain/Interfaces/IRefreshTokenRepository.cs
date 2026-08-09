using Touchliga.Domain.Entities;

namespace Touchliga.Domain.Interfaces;

public interface IRefreshTokenRepository
{
    Task AgregarAsync(RefreshToken token);

    Task<RefreshToken?> ObtenerAsync(string token);

    Task ActualizarAsync(RefreshToken token);
}
