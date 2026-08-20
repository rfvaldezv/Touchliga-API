using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Commands.Pago.Editar;
using Touchliga.Application.Common.Interfaces;

namespace Touchliga.Application.Handlers.Pago.Editar;

public sealed class EditarPagoCommandHandler : IRequestHandler<EditarPagoCommand, Unit>
{
    private readonly IPagoRepository _pagos;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public EditarPagoCommandHandler(
        IPagoRepository pagos,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _pagos = pagos;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(EditarPagoCommand request, CancellationToken cancellationToken)
    {
        var pago = await _pagos.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Pago");

        pago.Editar(
            request.Monto,
            request.MetodoPago,
            request.FechaPago,
            request.Referencia,
            _currentUser.UserId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
