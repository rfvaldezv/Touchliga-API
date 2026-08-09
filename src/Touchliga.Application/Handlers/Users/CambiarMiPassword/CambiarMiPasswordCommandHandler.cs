using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Users.Commands.CambiarMiPassword;
using Touchliga.Application.Common.Interfaces;
using Touchliga.Application.Authentication.Interfaces;

namespace Touchliga.Application.Handlers.Users.CambiarMiPassword;

public sealed class CambiarMiPasswordCommandHandler : IRequestHandler<CambiarMiPasswordCommand, Unit>
{
    private readonly IUsuarioRepository _usuarios;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IPasswordHasher _passwordHasher;

    public CambiarMiPasswordCommandHandler(
        IUsuarioRepository usuarios,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser,
        IPasswordHasher passwordHasher)
    {
        _usuarios = usuarios;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _passwordHasher = passwordHasher;
    }

    public async Task<Unit> Handle(CambiarMiPasswordCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarios.ObtenerPorIdAsync(_currentUser.UserId)
            ?? throw new EntityNotFoundException("Usuario");

        if (!_passwordHasher.Verify(request.PasswordActual, usuario.PasswordHash))
            throw new DomainException("Tu contraseña actual no es correcta.");

        if (string.IsNullOrWhiteSpace(request.PasswordNueva) || request.PasswordNueva.Length < 6)
            throw new DomainException("La nueva contraseña debe tener al menos 6 caracteres.");

        var nuevoHash = _passwordHasher.Hash(request.PasswordNueva);
        usuario.RestablecerPassword(nuevoHash, _currentUser.UserId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
