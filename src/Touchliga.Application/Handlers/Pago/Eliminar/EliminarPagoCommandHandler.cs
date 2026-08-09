using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Commands.Pago.Eliminar;

namespace Touchliga.Application.Handlers.Pago.Eliminar;

public sealed class EliminarPagoCommandHandler : IRequestHandler<EliminarPagoCommand, Unit>
{
    private readonly IPagoRepository _pagos;
    private readonly IUnitOfWork _unitOfWork;

    public EliminarPagoCommandHandler(IPagoRepository pagos, IUnitOfWork unitOfWork)
    {
        _pagos = pagos;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(EliminarPagoCommand request, CancellationToken cancellationToken)
    {
        var pago = await _pagos.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Pago");

        _pagos.Eliminar(pago);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
