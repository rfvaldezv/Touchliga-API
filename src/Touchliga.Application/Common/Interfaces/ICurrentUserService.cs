namespace Touchliga.Application.Common.Interfaces;

public interface ICurrentUserService
{
    bool IsAuthenticated { get; }

    long UserId { get; }

    string Nombre { get; }

    string Correo { get; }
}
