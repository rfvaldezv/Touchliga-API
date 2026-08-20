using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Users.Commands.EditarInfoUsuario;
using Touchliga.Application.Common.Interfaces;
using Touchliga.Application.Common.Utils;

namespace Touchliga.Application.Handlers.Users.EditarInfoUsuario;

public sealed class EditarInfoUsuarioCommandHandler : IRequestHandler<EditarInfoUsuarioCommand, Unit>
{
    private readonly IUsuarioRepository _usuarios;
    private readonly ICredencialAlternaRepository _credencialesAlternas;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IEmailService _email;
    private readonly IPaisRepository _paises;
    private readonly IEstadoRepository _estados;
    private readonly ICiudadRepository _ciudades;

    public EditarInfoUsuarioCommandHandler(
        IUsuarioRepository usuarios,
        ICredencialAlternaRepository credencialesAlternas,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser,
        IEmailService email,
        IPaisRepository paises,
        IEstadoRepository estados,
        ICiudadRepository ciudades)
    {
        _usuarios = usuarios;
        _credencialesAlternas = credencialesAlternas;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _email = email;
        _paises = paises;
        _estados = estados;
        _ciudades = ciudades;
    }

    public async Task<Unit> Handle(EditarInfoUsuarioCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarios.ObtenerPorIdAsync(request.UsuarioId)
            ?? throw new EntityNotFoundException("Usuario");

        // Si esta persona está VINCULADA a otra cuenta (su correo/
        // contraseña de siempre son la credencial alterna de otro
        // participante), y aquí se le está cambiando el correo, hay
        // que actualizar también esa credencial alterna -- si no, se
        // queda con el correo viejo y ya no la va a poder usar para
        // entrar.
        var correoAnterior = usuario.Correo.Value;

        usuario.ActualizarInfoContacto(
            request.Nombre,
            request.Apellidos,
            request.Telefono,
            request.Correo,
            request.CiudadId,
            request.PaisId,
            request.EstadoId,
            _currentUser.UserId);

        if (usuario.EsCuentaVinculada && !string.Equals(correoAnterior, request.Correo, StringComparison.OrdinalIgnoreCase))
        {
            var credencialAlterna = await _credencialesAlternas.ObtenerPorCorreoAsync(correoAnterior, cancellationToken);
            if (credencialAlterna != null)
            {
                credencialAlterna.Actualizar(usuario.Correo, credencialAlterna.PasswordHash, _currentUser.UserId);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Se espera, pero IEmailService ya trae su propio límite de 10s
        // (ver SmtpEmailService) -- nunca deja colgado el guardado, y
        // sus propios errores nunca tronan esta acción (los atrapa
        // internamente).
        var pais = request.PaisId.HasValue ? await _paises.ObtenerPorIdAsync(request.PaisId.Value, cancellationToken) : null;
        var estado = request.EstadoId.HasValue ? await _estados.ObtenerPorIdAsync(request.EstadoId.Value, cancellationToken) : null;
        var ciudad = request.CiudadId.HasValue ? await _ciudades.ObtenerPorIdAsync(request.CiudadId.Value, cancellationToken) : null;

        var cuerpo = PlantillaCorreoParticipante.InformacionActualizada(
            nombreCompleto: $"{request.Nombre} {request.Apellidos}",
            correo: request.Correo,
            telefono: request.Telefono,
            ciudad: ciudad?.Nombre,
            estado: estado?.Nombre,
            pais: pais?.Nombre);

        await _email.EnviarAsync(request.Correo, "Tu información en Touchliga se actualizó", cuerpo, cancellationToken);

        return Unit.Value;
    }
}
