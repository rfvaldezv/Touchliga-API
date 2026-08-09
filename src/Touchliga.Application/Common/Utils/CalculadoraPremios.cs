using Touchliga.Domain.Entities;
using Touchliga.Application.DTOs;

namespace Touchliga.Application.Common.Utils;

/// <summary>
/// Reparte los premios configurados entre los participantes, según
/// su posición en la clasificación (ya venga ordenada por puntos de
/// una jornada, o el acumulado de toda la temporada).
///
/// Regla de empates: si dos o más participantes empatan en puntos,
/// se suman los premios de TODAS las posiciones que ocupan entre
/// todos, y se reparte ese total en partes iguales. Ej.: si 2
/// personas empatan en 2° lugar, se suma premio(2°)+premio(3°) y se
/// divide entre 2 — cada una recibe esa mitad, en efectivo (dividir
/// un regalo físico no tiene sentido, así que un empate siempre se
/// resuelve en efectivo).
/// </summary>
public static class CalculadoraPremios
{
    public static List<GanadorPremioDto> Calcular(
        IReadOnlyList<(long UsuarioId, string Nombre, int Puntos)> participantesOrdenadosDesc,
        IReadOnlyList<ConfiguracionPremio> premiosPorPosicion)
    {
        var resultado = new List<GanadorPremioDto>();

        if (participantesOrdenadosDesc.Count == 0 || premiosPorPosicion.Count == 0)
            return resultado;

        var premiosPorPos = premiosPorPosicion.ToDictionary(p => p.Posicion);
        var maxPosicionConfigurada = premiosPorPos.Keys.Max();

        var i = 0;

        while (i < participantesOrdenadosDesc.Count && i < maxPosicionConfigurada)
        {
            var puntosActuales = participantesOrdenadosDesc[i].Puntos;

            var empatados = participantesOrdenadosDesc
                .Skip(i)
                .TakeWhile(p => p.Puntos == puntosActuales)
                .ToList();

            var n = empatados.Count;

            var premiosInvolucrados = Enumerable.Range(i + 1, n)
                .Where(pos => premiosPorPos.ContainsKey(pos))
                .Select(pos => premiosPorPos[pos])
                .ToList();

            if (premiosInvolucrados.Count == 0)
            {
                i += n;
                continue;
            }

            var montoTotal = premiosInvolucrados.Sum(p => p.Monto);
            var montoPorPersona = Math.Round(montoTotal / n, 2);
            var huboEmpate = n > 1;

            resultado.Add(new GanadorPremioDto
            {
                PosicionDesde = i + 1,
                PosicionHasta = i + n,
                Participantes = empatados
                    .Select(e => new GanadorParticipanteDto
                    {
                        UsuarioId = e.UsuarioId,
                        Nombre = e.Nombre,
                        Puntos = e.Puntos,
                        MontoSugerido = montoPorPersona,
                    })
                    .ToList(),
                TipoPremio = huboEmpate ? "Efectivo" : premiosInvolucrados[0].TipoPremio,
                Descripcion = huboEmpate ? null : premiosInvolucrados[0].Descripcion,
                HuboEmpate = huboEmpate,
            });

            i += n;
        }

        return resultado;
    }
}
