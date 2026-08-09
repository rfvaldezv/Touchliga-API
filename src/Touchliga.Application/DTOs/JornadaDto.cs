namespace Touchliga.Application.DTOs;

public sealed class JornadaDto
{
    public long Id { get; set; }

    public long TemporadaId { get; set; }

    public string Codigo { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public string Descripcion { get; set; } = string.Empty;

    public int Numero { get; set; }

    public DateTime FechaCierre { get; set; }

    public bool Cerrada { get; set; }

    public bool Activo { get; set; }
}
