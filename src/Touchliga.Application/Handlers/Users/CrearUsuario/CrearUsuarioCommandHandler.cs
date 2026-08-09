using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Entities;
using Touchliga.Domain.ValueObjects;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Users.Commands.CrearUsuario;
using Touchliga.Application.Authentication.Interfaces;

namespace Touchliga.Application.Handlers.Users.CrearUsuario;

public sealed class CrearUsuarioCommandHandler : IRequestHandler<CrearUsuarioCommand, long>
{
    private readonly IUsuarioRepository _usuarios;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public CrearUsuarioCommandHandler(
        IUsuarioRepository usuarios,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork)
    {
        _usuarios = usuarios;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<long> Handle(CrearUsuarioCommand request, CancellationToken cancellationToken)
    {
        if (await _usuarios.ExisteCorreoAsync(request.Correo))
            throw new BusinessException("Ya existe un usuario con ese correo.");

        var usuario = Usuario.CrearParticipante(
            request.Nombre,
            request.Apellidos,
            request.Telefono,
            Email.Create(request.Correo),
            _passwordHasher.Hash(request.Password),
            request.Sexo,
            request.InvitadoPorId,
            request.CiudadId,
            request.PaisId,
            request.EstadoId,
            request.InvitadoPorId);

        usuario.ConfirmarCorreo();

        await _usuarios.AgregarAsync(usuario);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return usuario.Id;
    }
}
