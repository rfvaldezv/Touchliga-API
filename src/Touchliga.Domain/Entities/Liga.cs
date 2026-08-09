using Touchliga.Domain.Common;
using Touchliga.Domain.Exceptions;

namespace Touchliga.Domain.Entities
{
    public sealed class Liga : BaseCatalogEntity
    {
        private Liga()
        {
        }

        public static Liga Crear(
            string codigo,
            string nombre,
            string? descripcion,
            bool activo,
            long usuarioAlta)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new DomainException("El código de la liga es obligatorio.");

            if (string.IsNullOrWhiteSpace(nombre))
                throw new DomainException("El nombre de la liga es obligatorio.");

            var liga = new Liga();

            liga.EstablecerCodigo(codigo);
            liga.EstablecerNombre(nombre);
            liga.EstablecerDescripcion(descripcion);

            liga.UsuarioAltaId = usuarioAlta;
            liga.Activo = activo;

            return liga;
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
