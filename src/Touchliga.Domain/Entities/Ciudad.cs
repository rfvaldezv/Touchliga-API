using Touchliga.Domain.Common;
using Touchliga.Domain.Exceptions;

namespace Touchliga.Domain.Entities
{
    public sealed class Ciudad : BaseCatalogEntity
    {
        private Ciudad()
        {
        }

        public long PaisId { get; private set; }

        public long EstadoId { get; private set; }

        public static Ciudad Crear(
            string codigo,
            string nombre,
            string? descripcion,
            long paisId,
            long estadoId,
            bool activo,
            long usuarioAlta)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new DomainException("El código es obligatorio.");

            if (string.IsNullOrWhiteSpace(nombre))
                throw new DomainException("El nombre es obligatorio.");

            if (paisId <= 0)
                throw new DomainException("El país es obligatorio.");

            if (estadoId <= 0)
                throw new DomainException("El estado es obligatorio.");

            var entidad = new Ciudad();

            entidad.EstablecerCodigo(codigo);
            entidad.EstablecerNombre(nombre);
            entidad.EstablecerDescripcion(descripcion);
            entidad.PaisId = paisId;
            entidad.EstadoId = estadoId;

            entidad.UsuarioAltaId = usuarioAlta;
            entidad.Activo = activo;

            return entidad;
        }

        public void ActualizarDatos(
            string nombre,
            string? descripcion,
            long paisId,
            long estadoId,
            bool activo,
            long usuarioId)
        {
            if (paisId <= 0)
                throw new DomainException("El país es obligatorio.");

            if (estadoId <= 0)
                throw new DomainException("El estado es obligatorio.");

            Actualizar(nombre, descripcion, usuarioId);
            PaisId = paisId;
            EstadoId = estadoId;

            if (activo && !Activo) Activar(usuarioId);

            if (!activo && Activo) Desactivar(usuarioId);
        }
    }
}
