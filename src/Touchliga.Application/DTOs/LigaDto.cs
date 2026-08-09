namespace Touchliga.Application.DTOs;

public sealed class LigaDto
{
    public long Id { get; set; }

    public string Codigo { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public string Descripcion { get; set; } = string.Empty;

    public bool Activo { get; set; }

    public decimal Cuota { get; set; }

}
