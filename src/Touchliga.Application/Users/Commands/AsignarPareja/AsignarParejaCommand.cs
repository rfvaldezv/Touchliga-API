using MediatR;

namespace Touchliga.Application.Users.Commands.AsignarPareja;

/// <summary>Vincula (o desvincula, mandando ParejaId=null) a un
/// participante con otro como pareja/equipo -- puramente visual,
/// nunca afecta pronósticos, puntos ni autenticación de ninguno de
/// los 2.</summary>
public sealed record AsignarParejaCommand(long UsuarioId, long? ParejaId, string? NombreEquipo) : IRequest<Unit>;
