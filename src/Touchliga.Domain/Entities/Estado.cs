using Touchliga.Domain.Common;
using Touchliga.Domain.Exceptions;

namespace Touchliga.Domain.Entities
{
    public sealed class Estado : BaseCatalogEntity
    {
        private Estado()
        {
        }

        public long PaisId { get; private set; }

        public static Estado Crear(
            string codigo,
            string nombre,
            string? descripcion,
            long paisId,
            bool activo,
            long usuarioAlta)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new DomainException("El código es obligatorio.");

            if (string.IsNullOrWhiteSpace(nombre))
                throw new DomainException("El nombre es obligatorio.");

            if (paisId <= 0)
                throw new DomainException("El país es obligatorio.");

            var entidad = new Estado();

            entidad.EstablecerCodigo(codigo);
            entidad.EstablecerNombre(nombre);
            entidad.EstablecerDescripcion(descripcion);
            entidad.PaisId = paisId;

            entidad.UsuarioAltaId = usuarioAlta;
            entidad.Activo = activo;

            return entidad;
        }

        public void ActualizarDatos(
            string nombre,
            string? descripcion,
            long paisId,
            bool activo,
            long usuarioId)
        {
            if (paisId <= 0)
                throw new DomainException("El país es obligatorio.");

            Actualizar(nombre, descripcion, usuarioId);
            PaisId = paisId;

            if (activo && !Activo) Activar(usuarioId);

            if (!activo && Activo) Desactivar(usuarioId);
        }
    }
}
