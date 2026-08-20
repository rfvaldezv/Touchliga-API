using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Entities;
using Touchliga.Domain.ValueObjects;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Users.Commands.CrearUsuario;
using Touchliga.Application.Authentication.Interfaces;
using Touchliga.Application.Common.Interfaces;
using Touchliga.Application.Common.Utils;

namespace Touchliga.Application.Handlers.Users.CrearUsuario;

public sealed class CrearUsuarioCommandHandler : IRequestHandler<CrearUsuarioCommand, long>
{
    private readonly IUsuarioRepository _usuarios;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _email;
    private readonly IPaisRepository _paises;
    private readonly IEstadoRepository _estados;
    private readonly ICiudadRepository _ciudades;

    public CrearUsuarioCommandHandler(
        IUsuarioRepository usuarios,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork,
        IEmailService email,
        IPaisRepository paises,
        IEstadoRepository estados,
        ICiudadRepository ciudades)
    {
        _usuarios = usuarios;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
        _email = email;
        _paises = paises;
        _estados = estados;
        _ciudades = ciudades;
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

        // Se espera, pero IEmailService ya trae su propio límite de 10s
        // (ver SmtpEmailService) -- nunca deja colgada el alta, y sus
        // propios errores nunca la tronan (los atrapa internamente).
        var pais = await _paises.ObtenerPorIdAsync(request.PaisId, cancellationToken);
        var estado = await _estados.ObtenerPorIdAsync(request.EstadoId, cancellationToken);
        var ciudad = await _ciudades.ObtenerPorIdAsync(request.CiudadId, cancellationToken);

        var cuerpo = PlantillaCorreoParticipante.Bienvenida(
            nombreCompleto: $"{request.Nombre} {request.Apellidos}",
            correo: request.Correo,
            passwordTemporal: request.Password,
            telefono: request.Telefono,
            ciudad: ciudad?.Nombre,
            estado: estado?.Nombre,
            pais: pais?.Nombre);

        await _email.EnviarAsync(request.Correo, "¡Bienvenido a Touchliga! 🏈", cuerpo, cancellationToken);

        return usuario.Id;
    }
}
