using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.Commands.Premio.GuardarConfiguracion;
using Touchliga.Application.Common.Interfaces;
using DomainEntity = Touchliga.Domain.Entities.ConfiguracionPremio;

namespace Touchliga.Application.Handlers.Premio.GuardarConfiguracion;

public sealed class GuardarConfiguracionPremiosCommandHandler
    : IRequestHandler<GuardarConfiguracionPremiosCommand, Unit>
{
    private readonly IConfiguracionPremioRepository _premios;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public GuardarConfiguracionPremiosCommandHandler(
        IConfiguracionPremioRepository premios,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _premios = premios;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(GuardarConfiguracionPremiosCommand request, CancellationToken cancellationToken)
    {
        var existentes = await _premios.ObtenerPorTemporadaYAmbitoAsync(
            request.TemporadaId, request.Ambito, cancellationToken);
        var existentesPorPosicion = existentes.ToDictionary(p => p.Posicion);

        foreach (var item in request.Premios)
        {
            if (existentesPorPosicion.TryGetValue(item.Posicion, out var existente))
            {
                existente.Editar(item.TipoPremio, item.Monto, item.Descripcion, _currentUser.UserId);
                _premios.Actualizar(existente);
            }
            else
            {
                var nuevo = DomainEntity.Crear(
                    request.TemporadaId,
                    request.Ambito,
                    item.Posicion,
                    item.TipoPremio,
                    item.Monto,
                    item.Descripcion,
                    _currentUser.UserId);

                await _premios.AgregarAsync(nuevo, cancellationToken);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
