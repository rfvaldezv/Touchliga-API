namespace Touchliga.Application.DTOs;

public sealed class EstadisticasParticipanteDto
{
    /// <summary>Jornadas cerradas seguidas (contando desde la mas
    /// reciente hacia atras) donde sumo al menos 1 punto.</summary>
    public int RachaActual { get; set; }

    /// <summary>"Mejorando", "Estable" o "Bajando" -- comparando la
    /// ultima jornada cerrada contra la anterior.</summary>
    public string Tendencia { get; set; } = "Estable";

    public int PosicionActual { get; set; }
    public int? PosicionAnterior { get; set; }

    /// <summary>Positivo = subio lugares, negativo = bajo, 0 = igual.</summary>
    public int MovimientoPosiciones { get; set; }

    /// <summary>Cuantas veces ha quedado en el top 3 de una jornada.</summary>
    public int VecesEnPodio { get; set; }

    public int PronosticosAcertados { get; set; }
    public int PronosticosFallados { get; set; }

    /// <summary>Null si el participante no ha registrado un equipo
    /// favorito en su Perfil.</summary>
    public string? EquipoFavoritoNombre { get; set; }

    /// <summary>Ultimos partidos del equipo favorito, mas reciente
    /// primero: "G" (gano), "P" (perdio) -- en NFL no hay empates.</summary>
    public List<string> FormaEquipoFavorito { get; set; } = new();
}
