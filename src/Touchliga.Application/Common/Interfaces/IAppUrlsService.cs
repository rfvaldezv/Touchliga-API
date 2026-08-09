namespace Touchliga.Application.Common.Interfaces;

public interface IAppUrlsService
{
    /// <summary>
    /// Ej. "https://rfvaldezv-001-site27.ltempurl.com" — se calcula
    /// de la petición real que está entrando, así que nunca hay que
    /// tocar código si el dominio cambia más adelante.
    /// </summary>
    string BaseUrlPublica { get; }
}
