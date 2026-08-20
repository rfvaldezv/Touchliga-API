using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.DTOs;
using Touchliga.Application.Queries.Reportes.GetParticipantesPendientes;

namespace Touchliga.Application.Handlers.Reportes.GetParticipantesPendientes;

public sealed class GetParticipantesPendientesQueryHandler
    : IRequestHandler<GetParticipantesPendientesQuery, List<ParticipantePendienteDto>>
{
    private readonly IReportesRepository _reportes;

    public GetParticipantesPendientesQueryHandler(IReportesRepository reportes)
    {
        _reportes = reportes;
    }

    public async Task<List<ParticipantePendienteDto>> Handle(
        GetParticipantesPendientesQuery request, CancellationToken cancellationToken)
    {
        var pendientes = await _reportes.ObtenerParticipantesPendientesAsync(request.JornadaId, cancellationToken);

        return pendientes.Select(p => new ParticipantePendienteDto
        {
            UsuarioId = p.UsuarioId,
            Nombre = p.Nombre,
            Correo = p.Correo,
            Telefono = p.Telefono,
            PartidosCapturados = p.PartidosCapturados,
            TotalPartidos = p.TotalPartidos
        }).ToList();
    }
}
