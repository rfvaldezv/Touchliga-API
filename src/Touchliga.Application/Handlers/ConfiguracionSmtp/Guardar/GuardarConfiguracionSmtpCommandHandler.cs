using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.Common.Interfaces;
using Touchliga.Application.Commands.ConfiguracionSmtp.Guardar;
using DomainEntity = Touchliga.Domain.Entities.ConfiguracionSmtp;

namespace Touchliga.Application.Handlers.ConfiguracionSmtp.Guardar;

public sealed class GuardarConfiguracionSmtpCommandHandler : IRequestHandler<GuardarConfiguracionSmtpCommand, Unit>
{
    private readonly IConfiguracionSmtpRepository _configuraciones;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public GuardarConfiguracionSmtpCommandHandler(
        IConfiguracionSmtpRepository configuraciones,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _configuraciones = configuraciones;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(GuardarConfiguracionSmtpCommand request, CancellationToken cancellationToken)
    {
        var existente = await _configuraciones.ObtenerAsync(cancellationToken);

        if (existente != null)
        {
            existente.Editar(
                request.Habilitado,
                request.Host,
                request.Port,
                request.Username,
                request.Password,
                request.FromEmail,
                request.FromName,
                _currentUser.UserId);

            _configuraciones.Actualizar(existente);
        }
        else
        {
            var nueva = DomainEntity.Crear(
                request.Habilitado,
                request.Host,
                request.Port,
                request.Username,
                request.Password,
                request.FromEmail,
                request.FromName,
                _currentUser.UserId);

            await _configuraciones.AgregarAsync(nueva, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
