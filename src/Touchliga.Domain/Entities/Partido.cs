using Touchliga.Domain.Common;
using Touchliga.Domain.Exceptions;

namespace Touchliga.Domain.Entities
{
    public sealed class Partido : AggregateRoot
    {
        private Partido()
        {
        }

        public long JornadaId { get; private set; }

        public long EquipoLocalId { get; private set; }

        public long EquipoVisitanteId { get; private set; }

        public DateTime FechaHora { get; private set; }

        public long? CanchaId { get; private set; }

        public int? GolesLocal { get; private set; }

        public int? GolesVisitante { get; private set; }

        /// <summary>El administrador marca UN partido por jornada
        /// como el de la caja de desempate (suma de puntos total).</summary>
        public bool EsDesempate { get; private set; }

        public bool TieneResultado => GolesLocal.HasValue && GolesVisitante.HasValue;

        /// <summary>Suma real de puntos del partido, solo disponible
        /// una vez capturado el resultado — es contra este valor que
        /// se compara la caja de desempate.</summary>
        public int? TotalPuntosReal => TieneResultado ? GolesLocal!.Value + GolesVisitante!.Value : null;

        /// <summary>Diferencia real de puntos del partido (margen de
        /// victoria) — segundo diferenciador de la caja de desempate,
        /// junto con TotalPuntosReal.</summary>
        public int? DiferenciaPuntosReal => TieneResultado ? Math.Abs(GolesLocal!.Value - GolesVisitante!.Value) : null;

        public static Partido Crear(
            long jornadaId,
            long equipoLocalId,
            long equipoVisitanteId,
            DateTime fechaHora,
            long? canchaId,
            long usuarioAlta)
        {
            if (jornadaId <= 0)
                throw new DomainException("La jornada es obligatoria.");

            if (equipoLocalId <= 0 || equipoVisitanteId <= 0)
                throw new DomainException("Ambos equipos son obligatorios.");

            if (equipoLocalId == equipoVisitanteId)
                throw new DomainException("El equipo local y el visitante no pueden ser el mismo.");

            return new Partido
            {
                JornadaId = jornadaId,
                EquipoLocalId = equipoLocalId,
                EquipoVisitanteId = equipoVisitanteId,
                FechaHora = fechaHora,
                CanchaId = canchaId,
                UsuarioAltaId = usuarioAlta,
                Activo = true
            };
        }

        /// <summary>
        /// Corrige los datos capturados al dar de alta el partido
        /// (equipos, fecha/hora, cancha) — para cuando el admin se
        /// equivocó al crearlo. No toca el marcador, eso sigue
        /// siendo CapturarResultado.
        /// </summary>
        public void Editar(
            long equipoLocalId,
            long equipoVisitanteId,
            DateTime fechaHora,
            long? canchaId,
            long usuarioId)
        {
            if (equipoLocalId <= 0 || equipoVisitanteId <= 0)
                throw new DomainException("Ambos equipos son obligatorios.");

            if (equipoLocalId == equipoVisitanteId)
                throw new DomainException("El equipo local y el visitante no pueden ser el mismo.");

            EquipoLocalId = equipoLocalId;
            EquipoVisitanteId = equipoVisitanteId;
            FechaHora = fechaHora;
            CanchaId = canchaId;

            MarcarModificado(usuarioId);
        }

        /// <summary>
        /// Captura el resultado real del partido (lo hace un
        /// administrador manualmente). Se puede volver a capturar
        /// para corregir un error, siempre que la jornada aún no
        /// esté cerrada (esa validación la hace el caso de uso,
        /// que sí tiene acceso a la Jornada).
        /// </summary>
        public void CapturarResultado(
            int golesLocal,
            int golesVisitante,
            long usuarioId)
        {
            if (golesLocal < 0 || golesVisitante < 0)
                throw new DomainException("El marcador no puede ser negativo.");

            GolesLocal = golesLocal;
            GolesVisitante = golesVisitante;

            MarcarModificado(usuarioId);
        }

        /// <summary>El caso de uso ya validó que no haya otro partido
        /// marcado como desempate en la misma jornada antes de llamar
        /// esto (una regla de "solo uno" que cruza entidades vive
        /// mejor en el handler, que sí ve todos los partidos de la
        /// jornada de un jalón).</summary>
        public void MarcarComoDesempate(bool esDesempate, long usuarioId)
        {
            EsDesempate = esDesempate;
            MarcarModificado(usuarioId);
        }
    }
}
