using Touchliga.Domain.Common;
using Touchliga.Domain.Exceptions;

namespace Touchliga.Domain.Entities
{
    public sealed class Jornada : BaseCatalogEntity
    {
        private Jornada()
        {
        }

        public long TemporadaId { get; private set; }

        public int Numero { get; private set; }

        public DateTime FechaCierre { get; private set; }

        public bool Cerrada { get; private set; }

        public static Jornada Crear(
            long temporadaId,
            string codigo,
            string nombre,
            string? descripcion,
            int numero,
            DateTime fechaCierre,
            bool activo,
            long usuarioAlta)
        {
            if (temporadaId <= 0)
                throw new DomainException("La temporada es obligatoria.");

            if (string.IsNullOrWhiteSpace(codigo))
                throw new DomainException("El código es obligatorio.");

            if (string.IsNullOrWhiteSpace(nombre))
                throw new DomainException("El nombre es obligatorio.");

            if (numero <= 0)
                throw new DomainException("El número de jornada debe ser mayor a cero.");

            var jornada = new Jornada
            {
                TemporadaId = temporadaId,
                Numero = numero,
                FechaCierre = fechaCierre,
                Cerrada = false
            };

            jornada.EstablecerCodigo(codigo);
            jornada.EstablecerNombre(nombre);
            jornada.EstablecerDescripcion(descripcion);

            jornada.UsuarioAltaId = usuarioAlta;
            jornada.Activo = activo;

            return jornada;
        }

        public void ActualizarDatos(
            string nombre,
            string? descripcion,
            int numero,
            DateTime fechaCierre,
            bool activo,
            long usuarioId)
        {
            if (Cerrada)
                throw new DomainException("No se puede modificar una jornada ya cerrada.");

            if (numero <= 0)
                throw new DomainException("El número de jornada debe ser mayor a cero.");

            Actualizar(nombre, descripcion, usuarioId);

            Numero = numero;
            FechaCierre = fechaCierre;

            if (activo && !Activo) Activar(usuarioId);

            if (!activo && Activo) Desactivar(usuarioId);
        }

        /// <summary>
        /// Cierra la jornada: a partir de este momento ya no se pueden
        /// crear ni editar pronósticos de los partidos que contiene.
        /// El cálculo de puntos se realiza por separado, después de
        /// cerrar (ver Pronostico.CalcularPuntos), típicamente en el
        /// mismo caso de uso que invoca este método.
        /// </summary>
        public void Cerrar(long usuarioId)
        {
            if (Cerrada)
                throw new DomainException("La jornada ya está cerrada.");

            Cerrada = true;

            MarcarModificado(usuarioId);
        }

        /// <summary>
        /// Reabre una jornada cerrada — para ajustes de último
        /// momento (corregir un pronóstico o resultado capturado
        /// por error). Se puede volver a cerrar normalmente después.
        /// </summary>
        public void Abrir(long usuarioId)
        {
            if (!Cerrada)
                throw new DomainException("La jornada no está cerrada.");

            Cerrada = false;

            MarcarModificado(usuarioId);
        }
    }
}
