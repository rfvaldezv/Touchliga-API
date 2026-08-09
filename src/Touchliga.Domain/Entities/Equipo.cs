using Touchliga.Domain.Common;
using Touchliga.Domain.Exceptions;

namespace Touchliga.Domain.Entities
{
    public sealed class Equipo : BaseCatalogEntity
    {
        private Equipo()
        {
        }

        public string? EscudoUrl { get; private set; }

        public string? Apodo { get; private set; }

        public static Equipo Crear(
            string codigo,
            string nombre,
            string? descripcion,
            string? escudoUrl,
            string? apodo,
            bool activo,
            long usuarioAlta)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new DomainException("El código es obligatorio.");

            if (string.IsNullOrWhiteSpace(nombre))
                throw new DomainException("El nombre es obligatorio.");

            var entidad = new Equipo
            {
                EscudoUrl = string.IsNullOrWhiteSpace(escudoUrl) ? null : escudoUrl.Trim(),
                Apodo = string.IsNullOrWhiteSpace(apodo) ? null : apodo.Trim()
            };

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
            string? escudoUrl,
            string? apodo,
            bool activo,
            long usuarioId)
        {
            Actualizar(nombre, descripcion, usuarioId);

            EscudoUrl = string.IsNullOrWhiteSpace(escudoUrl) ? null : escudoUrl.Trim();
            Apodo = string.IsNullOrWhiteSpace(apodo) ? null : apodo.Trim();

            if (activo && !Activo) Activar(usuarioId);

            if (!activo && Activo) Desactivar(usuarioId);
        }
    }
}
