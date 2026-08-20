using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Users.Commands.RestablecerPassword;
using Touchliga.Application.Common.Interfaces;
using Touchliga.Application.Authentication.Interfaces;

namespace Touchliga.Application.Handlers.Users.RestablecerPassword;

public sealed class RestablecerPasswordCommandHandler : IRequestHandler<RestablecerPasswordCommand, string>
{
    private readonly IUsuarioRepository _usuarios;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IPasswordHasher _passwordHasher;

    public RestablecerPasswordCommandHandler(
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

    public async Task<string> Handle(RestablecerPasswordCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarios.ObtenerPorIdAsync(request.UsuarioId)
            ?? throw new EntityNotFoundException("Usuario");

        var nuevaPassword = string.IsNullOrWhiteSpace(request.NuevaPassword)
            ? GenerarPasswordAleatoria()
            : request.NuevaPassword.Trim();
        var hash = _passwordHasher.Hash(nuevaPassword);

        usuario.RestablecerPassword(hash, _currentUser.UserId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return nuevaPassword;
    }

    private static string GenerarPasswordAleatoria()
    {
        const string alfabeto = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789";
        var random = new Random();
        return new string(Enumerable.Range(0, 10).Select(_ => alfabeto[random.Next(alfabeto.Length)]).ToArray());
    }
}
