using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Touchliga.Persistence.Context;

public sealed class TouchligaDbContextFactory
    : IDesignTimeDbContextFactory<TouchligaDbContext>
{
    // Debe coincidir con el <UserSecretsId> de Touchliga.Api.csproj
    // (el que generó `dotnet user-secrets init --project src\Touchliga.Api`).
    // No es un secreto en sí — es solo el identificador de la carpeta
    // donde Windows/Linux guardan los secretos locales de este proyecto.
    private const string UserSecretsId = "0295b193-3fb0-45a3-997b-b7f706f2a540";

    public TouchligaDbContext CreateDbContext(string[] args)
    {
        var current = Directory.GetCurrentDirectory();

        var apiPath = Path.Combine(current, "src", "Touchliga.Api");

        if (!Directory.Exists(apiPath))
        {
            apiPath = Path.GetFullPath(Path.Combine(current, "..", "Touchliga.Api"));
        }

        var environment =
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            ?? "Development";

        var configuration = new ConfigurationBuilder()
            .SetBasePath(apiPath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddUserSecrets(UserSecretsId)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString) ||
            connectionString.StartsWith("__SET_VIA_"))
        {
            throw new InvalidOperationException(
                "No se encontró ConnectionStrings:DefaultConnection. " +
                "Configúrala con: dotnet user-secrets set \"ConnectionStrings:DefaultConnection\" \"...\" --project src\\Touchliga.Api");
        }

        var options = new DbContextOptionsBuilder<TouchligaDbContext>();

        options.UseSqlServer(connectionString);

        return new TouchligaDbContext(options.Options);
    }
}
