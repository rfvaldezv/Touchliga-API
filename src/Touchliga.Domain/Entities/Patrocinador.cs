using Touchliga.Domain.Common;
using Touchliga.Domain.Exceptions;

namespace Touchliga.Domain.Entities
{
    public sealed class Patrocinador : BaseCatalogEntity
    {
        private Patrocinador()
        {
        }

        public string ImagenUrl { get; private set; } = string.Empty;

        public string? EnlaceUrl { get; private set; }

        public int Orden { get; private set; }

        public static Patrocinador Crear(
            string codigo,
            string nombre,
            string? descripcion,
            string imagenUrl,
            string? enlaceUrl,
            int orden,
            bool activo,
            long usuarioAlta)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new DomainException("El código es obligatorio.");

            if (string.IsNullOrWhiteSpace(nombre))
                throw new DomainException("El nombre es obligatorio.");

            if (string.IsNullOrWhiteSpace(imagenUrl))
                throw new DomainException("La imagen del banner es obligatoria.");

            var patrocinador = new Patrocinador
            {
                ImagenUrl = imagenUrl.Trim(),
                EnlaceUrl = string.IsNullOrWhiteSpace(enlaceUrl) ? null : enlaceUrl.Trim(),
                Orden = orden
            };

            patrocinador.EstablecerCodigo(codigo);
            patrocinador.EstablecerNombre(nombre);
            patrocinador.EstablecerDescripcion(descripcion);

            patrocinador.UsuarioAltaId = usuarioAlta;
            patrocinador.Activo = activo;

            return patrocinador;
        }

        public void ActualizarDatos(
            string nombre,
            string? descripcion,
            string imagenUrl,
            string? enlaceUrl,
            int orden,
            bool activo,
            long usuarioId)
        {
            if (string.IsNullOrWhiteSpace(imagenUrl))
                throw new DomainException("La imagen del banner es obligatoria.");

            Actualizar(nombre, descripcion, usuarioId);

            ImagenUrl = imagenUrl.Trim();
            EnlaceUrl = string.IsNullOrWhiteSpace(enlaceUrl) ? null : enlaceUrl.Trim();
            Orden = orden;

            if (activo && !Activo) Activar(usuarioId);

            if (!activo && Activo) Desactivar(usuarioId);
        }
    }
}
