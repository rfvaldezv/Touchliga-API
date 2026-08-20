using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.DTOs;
using Touchliga.Application.Queries.ConfiguracionSmtp.Get;

namespace Touchliga.Application.Handlers.ConfiguracionSmtp.Get;

public sealed class GetConfiguracionSmtpQueryHandler : IRequestHandler<GetConfiguracionSmtpQuery, ConfiguracionSmtpDto?>
{
    private readonly IConfiguracionSmtpRepository _configuraciones;

    public GetConfiguracionSmtpQueryHandler(IConfiguracionSmtpRepository configuraciones)
    {
        _configuraciones = configuraciones;
    }

    public async Task<ConfiguracionSmtpDto?> Handle(GetConfiguracionSmtpQuery request, CancellationToken cancellationToken)
    {
        var config = await _configuraciones.ObtenerAsync(cancellationToken);

        if (config is null) return null;

        return new ConfiguracionSmtpDto
        {
            Habilitado = config.Habilitado,
            Host = config.Host,
            Port = config.Port,
            Username = config.Username,
            Password = config.Password,
            FromEmail = config.FromEmail,
            FromName = config.FromName
        };
    }
}
