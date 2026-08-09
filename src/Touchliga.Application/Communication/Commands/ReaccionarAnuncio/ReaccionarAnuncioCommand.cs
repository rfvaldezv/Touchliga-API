using MediatR;

namespace Touchliga.Application.Communication.Commands.ReaccionarAnuncio;

/// <summary>Si ya tenías ese mismo emoji puesto, se quita (alternar);
/// si tenías otro, se cambia; si no tenías ninguno, se agrega.</summary>
public sealed record ReaccionarAnuncioCommand(long AnuncioId, string Emoji) : IRequest<Unit>;
