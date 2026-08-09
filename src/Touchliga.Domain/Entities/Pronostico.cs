using Touchliga.Domain.Exceptions;

namespace Touchliga.Domain.Entities
{
    public sealed class Pronostico : Common.AggregateRoot
    {
        private Pronostico()
        {
        }

        public long PartidoId { get; private set; }

        public long UsuarioId { get; private set; }

        /// <summary>El equipo que el participante cree que va a
        /// ganar -- en Touchliga no se captura marcador, solo el
        /// ganador (a diferencia de FutLiga, que si captura goles).</summary>
        public long EquipoGanadorId { get; private set; }

        public int? Puntos { get; private set; }

        /// <summary>Solo tiene valor cuando el Partido esta marcado
        /// como el de la caja de desempate de la jornada -- la suma
        /// de puntos total que el participante predice para ese
        /// partido.</summary>
        public int? PuntosTotalesPredichos { get; private set; }

        /// <summary>Solo tiene valor cuando el Partido esta marcado
        /// como el de la caja de desempate -- la diferencia de
        /// puntos (margen de victoria) que el participante predice.
        /// Segundo diferenciador, junto con PuntosTotalesPredichos.</summary>
        public int? DiferenciaPuntosPredicha { get; private set; }

        /// <summary>1 si este participante quedo entre los mas
        /// cercanos (o empatado) al combinado real (suma+diferencia)
        /// del partido de desempate; 0 en cualquier otro caso,
        /// incluyendo partidos que no son de desempate. Se suma
        /// junto con Puntos al acumulado de la jornada.</summary>
        public int PuntosBono { get; private set; }

        public static Pronostico Crear(
            long partidoId,
            long usuarioId,
            long equipoGanadorId,
            int? puntosTotalesPredichos = null,
            int? diferenciaPuntosPredicha = null)
        {
            if (partidoId <= 0)
                throw new DomainException("El partido es obligatorio.");

            if (equipoGanadorId <= 0)
                throw new DomainException("Debes elegir un equipo ganador.");

            if (puntosTotalesPredichos.HasValue && puntosTotalesPredichos.Value < 0)
                throw new DomainException("La suma de puntos no puede ser negativa.");

            if (diferenciaPuntosPredicha.HasValue && diferenciaPuntosPredicha.Value < 0)
                throw new DomainException("La diferencia de puntos no puede ser negativa.");

            return new Pronostico
            {
                PartidoId = partidoId,
                UsuarioId = usuarioId,
                EquipoGanadorId = equipoGanadorId,
                PuntosTotalesPredichos = puntosTotalesPredichos,
                DiferenciaPuntosPredicha = diferenciaPuntosPredicha,
                PuntosBono = 0,
                UsuarioAltaId = usuarioId,
                Activo = true
            };
        }

        /// <summary>
        /// El pronostico se puede editar libremente hasta que la
        /// jornada del partido cierre (esa validacion la hace el
        /// caso de uso, que si tiene acceso a la Jornada).
        /// </summary>
        public void Actualizar(
            long equipoGanadorId,
            int? puntosTotalesPredichos,
            int? diferenciaPuntosPredicha,
            long usuarioId)
        {
            if (equipoGanadorId <= 0)
                throw new DomainException("Debes elegir un equipo ganador.");

            if (puntosTotalesPredichos.HasValue && puntosTotalesPredichos.Value < 0)
                throw new DomainException("La suma de puntos no puede ser negativa.");

            if (diferenciaPuntosPredicha.HasValue && diferenciaPuntosPredicha.Value < 0)
                throw new DomainException("La diferencia de puntos no puede ser negativa.");

            EquipoGanadorId = equipoGanadorId;
            PuntosTotalesPredichos = puntosTotalesPredichos;
            DiferenciaPuntosPredicha = diferenciaPuntosPredicha;

            MarcarModificado(usuarioId);
        }

        /// <summary>
        /// Reglas de puntuacion de Touchliga: solo hay ganador y
        /// perdedor (nunca empate en NFL) -- 1 punto si acerto quien
        /// gana, 0 si no.
        /// </summary>
        public void CalcularPuntos(long equipoGanadorReal)
        {
            Puntos = EquipoGanadorId == equipoGanadorReal ? 1 : 0;
        }

        /// <summary>El handler que procesa el resultado del partido
        /// de desempate llama esto una vez que ya determino, entre
        /// TODOS los pronosticos de ese partido, quien(es) quedaron
        /// mas cerca (empates incluidos, todos ganan el bono).</summary>
        public void AsignarPuntoBono(bool gano)
        {
            PuntosBono = gano ? 1 : 0;
        }
    }
}
