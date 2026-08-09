using Touchliga.Domain.Common;

namespace Touchliga.Domain.Entities;

public sealed class Sesion : AggregateRoot
{
    private Sesion()
    {
    }

    public long UsuarioId { get; private set; }

    public DateTime Inicio { get; private set; }

    public DateTime? Fin { get; private set; }

    public string DireccionIp { get; private set; } = string.Empty;

    public string Dispositivo { get; private set; } = string.Empty;

    public string SistemaOperativo { get; private set; } = string.Empty;

    public string Navegador { get; private set; } = string.Empty;

    public static Sesion Crear(
        long usuarioId,
        string ip,
        string dispositivo,
        string sistemaOperativo,
        string navegador,
        long usuarioAlta)
    {
        return new Sesion
        {
            UsuarioId = usuarioId,
            Inicio = DateTime.UtcNow,
            DireccionIp = ip,
            Dispositivo = dispositivo,
            SistemaOperativo = sistemaOperativo,
            Navegador = navegador,
            UsuarioAltaId = usuarioAlta,
            FechaAlta = DateTime.UtcNow
        };
    }

    public void Finalizar(long usuarioId)
    {
        Fin = DateTime.UtcNow;
        MarcarModificado(usuarioId);
    }
}
