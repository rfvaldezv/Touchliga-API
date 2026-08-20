namespace Touchliga.Application.Common.Interfaces;

/// <summary>
/// Genera el PDF de auditoría de una jornada: una tabla con cada
/// participante en una fila, y sus pronósticos de cada partido en
/// las columnas. Pensado para compartirse en el grupo de WhatsApp
/// como comprobante de lo que todos registraron.
/// </summary>
public interface IReporteAuditoriaPdfService
{
    byte[] Generar(
        string titulo,
        string subtitulo,
        List<string> columnas,
        List<(string participante, List<string> valores)> filas);
}
