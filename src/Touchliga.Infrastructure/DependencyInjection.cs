using Touchliga.Application.Authentication.Interfaces;
using Touchliga.Infrastructure.Authentication.Jwt;
using Touchliga.Infrastructure.Authentication.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Touchliga.Application.Common.Interfaces;
using Touchliga.Infrastructure.Authentication;
using Touchliga.Infrastructure.Email;
using Touchliga.Infrastructure.Push;
using Touchliga.Infrastructure.Pagos;
using Touchliga.Infrastructure.Reportes;

namespace Touchliga.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtOptions>(
            configuration.GetSection(JwtOptions.SectionName));

        services.AddScoped<IJwtService, JwtService>();

        services.AddScoped<IPasswordHasher, PasswordHasher>();

        services.AddScoped<IAuthenticationService, AuthenticationService>();

        services.AddHttpContextAccessor();

        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.Configure<EmailOptions>(
            configuration.GetSection(EmailOptions.SectionName));

        services.AddScoped<IEmailService, SmtpEmailService>();
        services.AddScoped<IReporteAuditoriaPdfService, QuestPdfReporteAuditoriaService>();

        services.Configure<PushOptions>(
            configuration.GetSection(PushOptions.SectionName));

        services.AddScoped<IPushNotificationService, FirebasePushNotificationService>();

        services.Configure<StripeOptions>(
            configuration.GetSection(StripeOptions.SectionName));

        services.AddScoped<IPagoStripeService, StripePagoService>();

        services.AddHttpContextAccessor();
        services.AddScoped<IAppUrlsService, AppUrlsService>();

        return services;
    }
}
