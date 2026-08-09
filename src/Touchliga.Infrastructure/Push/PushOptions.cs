namespace Touchliga.Infrastructure.Push;

/// <summary>Se llena desde la sección "Push" de appsettings/user-secrets.</summary>
public sealed class PushOptions
{
    public const string SectionName = "Push";

    /// <summary>
    /// Si es false, el servicio no manda nada de verdad — solo lo
    /// registra en el log. Igual que con el correo, así el resto de
    /// la app sigue funcionando aunque todavía no haya credenciales.
    /// </summary>
    public bool Habilitado { get; set; }

    /// <summary>
    /// El contenido COMPLETO del archivo JSON de la cuenta de
    /// servicio de Firebase (el que se descarga desde Configuración
    /// del proyecto → Cuentas de servicio → Generar nueva clave
    /// privada), pegado tal cual como valor de un solo secreto — así
    /// no hay que manejar un archivo aparte en el servidor.
    /// </summary>
    public string CredencialesJson { get; set; } = string.Empty;
}
