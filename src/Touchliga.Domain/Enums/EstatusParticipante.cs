namespace Touchliga.Domain.Enums;

/// <summary>
/// Igual que el sistema anterior (A/X/Z) pero con nombres explícitos.
/// Activo mantiene Usuario.Activo=true; los otros dos lo ponen en
/// false, así que todo el código existente que filtra por Activo
/// sigue funcionando igual sin cambios.
/// </summary>
public enum EstatusParticipante
{
    Activo = 1,
    InactivoTemporal = 2,
    BajaDefinitiva = 3
}
