using Touchliga.Domain.Common;
using Touchliga.Domain.Exceptions;

namespace Touchliga.Domain.Entities
{
    public sealed class Pais : BaseCatalogEntity
    {
        private Pais()
        {
        }

        public static Pais Crear(
            string codigo,
            string nombre,
            string? descripcion,
            bool activo,
            long usuarioAlta)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new DomainException("El código es obligatorio.");

            if (string.IsNullOrWhiteSpace(nombre))
                throw new DomainException("El nombre es obligatorio.");

            var entidad = new Pais();

            entidad.EstablecerCodigo(codigo);
            entidad.EstablecerNombre(nombre);
            entidad.EstablecerDescripcion(descripcion);

            entidad.UsuarioAltaId = usuarioAlta;
            entidad.Activo = activo;

            return entidad;
        }

        public void ActualizarDatos(
            string nombre,
            string? descripcion,
            bool activo,
            long usuarioId)
        {
            Actualizar(nombre, descripcion, usuarioId);

            if (activo && !Activo) Activar(usuarioId);

            if (!activo && Activo) Desactivar(usuarioId);
        }
    }
}
