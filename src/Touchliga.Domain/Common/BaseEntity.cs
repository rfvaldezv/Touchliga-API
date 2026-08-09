    namespace Touchliga.Domain.Common;

    public abstract class BaseEntity
    {
        public long Id { get; protected set; }

        public bool Activo { get; protected set; } = true;

        public DateTime FechaAlta { get; protected set; } = DateTime.UtcNow;

        public long? UsuarioAltaId { get; protected set; }

        public DateTime? FechaModificacion { get; protected set; }

        public long? UsuarioModificacionId { get; protected set; }

        public byte[] RowVersion { get; protected set; } = Array.Empty<byte>();

        private void RegistrarModificacion(long usuarioId)
        {
            FechaModificacion = DateTime.UtcNow;
            UsuarioModificacionId = usuarioId;
        }
        
        public void Activar(long usuarioId)
        {
         Activo = true;
            RegistrarModificacion(usuarioId);
        }

        public void Desactivar(long usuarioId)
        {
            Activo = false;
            RegistrarModificacion(usuarioId);
        }

        public void MarcarModificado(long usuarioId)
        {
            RegistrarModificacion(usuarioId);
        }
    }
