using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Commands.Pronostico.GuardarLote;
using Touchliga.Application.Common.Interfaces;
using DomainEntity = Touchliga.Domain.Entities.Pronostico;

namespace Touchliga.Application.Handlers.Pronostico.GuardarLote;

public sealed class GuardarPronosticosLoteCommandHandler : IRequestHandler<GuardarPronosticosLoteCommand, bool>
{
    private readonly IPronosticoRepository _pronosticos;
    private readonly IPartidoRepository _partidos;
    private readonly IJornadaRepository _jornadas;
    private readonly IUsuarioRepository _usuarios;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IEmailService _email;

    public GuardarPronosticosLoteCommandHandler(
        IPronosticoRepository pronosticos,
        IPartidoRepository partidos,
        IJornadaRepository jornadas,
        IUsuarioRepository usuarios,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser,
        IEmailService email)
    {
        _pronosticos = pronosticos;
        _partidos = partidos;
        _jornadas = jornadas;
        _usuarios = usuarios;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _email = email;
    }

    public async Task<bool> Handle(GuardarPronosticosLoteCommand request, CancellationToken cancellationToken)
    {
        var jornada = await _jornadas.ObtenerPorIdAsync(request.JornadaId, cancellationToken)
            ?? throw new EntityNotFoundException("Jornada");

        if (jornada.Cerrada)
            throw new BusinessException("Ya no se pueden capturar ni editar pronósticos: la jornada está cerrada.");

        var usuarioId = _currentUser.UserId;

        // Se trae una sola vez, antes del ciclo, para saber (sin
        // otra consulta por partido) cuáles de estos ya tienen
        // resultado real capturado — pasa seguido al capturar
        // pronósticos "hacia atrás", de partidos que ya se jugaron.
        var partidosDeLaJornada = await _partidos.ObtenerPorJornadaAsync(request.JornadaId, cancellationToken);
        var partidosPorId = partidosDeLaJornada.ToDictionary(p => p.Id);

        foreach (var item in request.Pronosticos)
        {
            var existente = await _pronosticos.ObtenerPorPartidoYUsuarioAsync(
                item.PartidoId, usuarioId, cancellationToken);

            var tieneResultado = partidosPorId.TryGetValue(item.PartidoId, out var partidoDelItem)
                && partidoDelItem.TieneResultado;

            long? equipoGanadorReal = tieneResultado
                ? (partidoDelItem!.GolesLocal!.Value > partidoDelItem.GolesVisitante!.Value
                    ? partidoDelItem.EquipoLocalId
                    : partidoDelItem.EquipoVisitanteId)
                : null;

            if (existente != null)
            {
                existente.Actualizar(
                    item.EquipoGanadorId, item.PuntosTotalesPredichos, item.DiferenciaPuntosPredicha, usuarioId);

                if (equipoGanadorReal.HasValue)
                    existente.CalcularPuntos(equipoGanadorReal.Value);

                _pronosticos.Actualizar(existente);
            }
            else
            {
                var nuevo = DomainEntity.Crear(
                    item.PartidoId, usuarioId, item.EquipoGanadorId, item.PuntosTotalesPredichos, item.DiferenciaPuntosPredicha);

                if (equipoGanadorReal.HasValue)
                    nuevo.CalcularPuntos(equipoGanadorReal.Value);

                await _pronosticos.AgregarAsync(nuevo, cancellationToken);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // ¿Ya cubrió TODOS los partidos de esta jornada? Si sí, se
        // manda la confirmación — una sola vez, sin importar cuántos
        // partidos traía este lote.
        var partidoIds = partidosDeLaJornada.Select(p => p.Id).ToList();

        var completa = partidoIds.Count > 0
            && await _pronosticos.ContarPorPartidoIdsYUsuarioAsync(partidoIds, usuarioId, cancellationToken)
                == partidoIds.Count;

        if (completa)
        {
            var usuario = await _usuarios.ObtenerPorIdAsync(usuarioId);

            if (usuario != null)
            {
                var cuerpo = $"""
                    <p>Hola {usuario.Nombre},</p>
                    <p>Confirmamos que tus pronósticos para la <strong>{jornada.Nombre}</strong> quedaron registrados completos ({partidoIds.Count} de {partidoIds.Count} partidos).</p>
                    <p>Si necesitas corregir alguno, puedes hacerlo desde la app mientras la jornada siga abierta.</p>
                    <p>¡Mucha suerte! ⚽</p>
                    <p style="color:#888;font-size:12px;">Touchliga — Pasión que nos une</p>
                    """;

                await _email.EnviarAsync(
                    usuario.Correo.Value,
                    $"Pronósticos confirmados — {jornada.Nombre}",
                    cuerpo,
                    cancellationToken);
            }
        }

        return completa;
    }
}
