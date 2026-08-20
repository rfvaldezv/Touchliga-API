using MediatR;

namespace Touchliga.Application.Commands.ConfiguracionSmtp.Guardar;

public sealed record GuardarConfiguracionSmtpCommand(
    bool Habilitado,
    string Host,
    int Port,
    string Username,
    string Password,
    string FromEmail,
    string FromName
) : IRequest<Unit>;
