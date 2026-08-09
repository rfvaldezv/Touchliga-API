using Touchliga.Domain.Common;
using Touchliga.Domain.Exceptions;

namespace Touchliga.Domain.Entities
{
    public sealed class Temporada : BaseCatalogEntity
    {
        private Temporada()
        {
        }

        public long LigaId { get; private set; }

        public DateTime FechaInicio { get; private set; }

        public DateTime FechaFin { get; private set; }

        public decimal Cuota { get; private set; }

        public static Temporada Crear(
            long ligaId,
            string codigo,
            string nombre,
            string? descripcion,
            DateTime fechaInicio,
            DateTime fechaFin,
            decimal cuota,
            bool activo,
            long usuarioAlta)
        {
            if (ligaId <= 0)
                throw new DomainException("La liga es obligatoria.");

            if (string.IsNullOrWhiteSpace(codigo))
                throw new DomainException("El código es obligatorio.");

            if (string.IsNullOrWhiteSpace(nombre))
                throw new DomainException("El nombre es obligatorio.");

            if (fechaFin < fechaInicio)
                throw new DomainException("La fecha de fin no puede ser anterior a la fecha de inicio.");

            if (cuota < 0)
                throw new DomainException("La cuota no puede ser negativa.");

            var temporada = new Temporada
            {
                LigaId = ligaId,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                Cuota = cuota
            };

            temporada.EstablecerCodigo(codigo);
            temporada.EstablecerNombre(nombre);
            temporada.EstablecerDescripcion(descripcion);

            temporada.UsuarioAltaId = usuarioAlta;
            temporada.Activo = activo;

            return temporada;
        }

        public void ActualizarDatos(
            string nombre,
            string? descripcion,
            DateTime fechaInicio,
            DateTime fechaFin,
            decimal cuota,
            bool activo,
            long usuarioId)
        {
            if (fechaFin < fechaInicio)
                throw new DomainException("La fecha de fin no puede ser anterior a la fecha de inicio.");

            if (cuota < 0)
                throw new DomainException("La cuota no puede ser negativa.");

            Actualizar(nombre, descripcion, usuarioId);

            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
            Cuota = cuota;

            if (activo && !Activo) Activar(usuarioId);

            if (!activo && Activo) Desactivar(usuarioId);
        }
    }
}
