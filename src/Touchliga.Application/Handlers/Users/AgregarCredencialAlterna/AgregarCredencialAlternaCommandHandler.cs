using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Entities;
using Touchliga.Domain.ValueObjects;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Authentication.Interfaces;
using Touchliga.Application.Common.Interfaces;
using Touchliga.Application.Users.Commands.AgregarCredencialAlterna;

namespace Touchliga.Application.Handlers.Users.AgregarCredencialAlterna;

public sealed class AgregarCredencialAlternaCommandHandler : IRequestHandler<AgregarCredencialAlternaCommand, Unit>
{
    private readonly IUsuarioRepository _usuarios;
    private readonly ICredencialAlternaRepository _credencialesAlternas;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public AgregarCredencialAlternaCommandHandler(
        IUsuarioRepository usuarios,
        ICredencialAlternaRepository credencialesAlternas,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _usuarios = usuarios;
        _credencialesAlternas = credencialesAlternas;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(AgregarCredencialAlternaCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarios.ObtenerPorIdAsync(request.UsuarioId)
            ?? throw new EntityNotFoundException("Usuario");

        // El correo SÍ puede ser el de otra cuenta principal existente
        // a propósito (ej. la esposa/esposo ya tiene su propio
        // participante, y ahora ese mismo correo también sirve, con
        // una contraseña distinta, para entrar a esta cuenta
        // vinculada) -- solo se evita el caso sin sentido de
        // vincularse con el propio correo de uno mismo.
        var propietarioDelCorreo = await _usuarios.ObtenerPorCorreoAsync(request.Correo);
        if (propietarioDelCorreo != null && propietarioDelCorreo.Id == request.UsuarioId)
            throw new BusinessException("Ese ya es el correo principal de este mismo participante.");

        var existente = await _credencialesAlternas.ObtenerPorUsuarioIdAsync(request.UsuarioId, cancellationToken);

        var otraConEseCorreo = await _credencialesAlternas.ObtenerPorCorreoAsync(request.Correo, cancellationToken);
        if (otraConEseCorreo != null && otraConEseCorreo.Id != existente?.Id)
            throw new BusinessException("Ese correo ya está en uso como credencial alterna de otro participante.");

        var hash = _passwordHasher.Hash(request.Password);

        if (existente != null)
        {
            // Ya tenía una -- se actualiza en el mismo registro (nuevo
            // correo/contraseña), en vez de borrar y crear otra.
            existente.Actualizar(Email.Create(request.Correo), hash, _currentUser.UserId);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        var nueva = CredencialAlterna.Crear(
            request.UsuarioId,
            Email.Create(request.Correo),
            hash,
            _currentUser.UserId);

        await _credencialesAlternas.AgregarAsync(nueva, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
