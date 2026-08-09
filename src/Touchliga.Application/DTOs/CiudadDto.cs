namespace Touchliga.Application.DTOs;

public sealed class CiudadDto
{
    public long Id { get; set; }

    public string Codigo { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public string Descripcion { get; set; } = string.Empty;

    public long PaisId { get; set; }

    public string PaisNombre { get; set; } = string.Empty;

    public long EstadoId { get; set; }

    public string EstadoNombre { get; set; } = string.Empty;

    public bool Activo { get; set; }
}
