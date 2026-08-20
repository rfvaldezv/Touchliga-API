using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.Common.Interfaces;
using Touchliga.Application.Users.Commands.QuitarCredencialAlterna;

namespace Touchliga.Application.Handlers.Users.QuitarCredencialAlterna;

public sealed class QuitarCredencialAlternaCommandHandler : IRequestHandler<QuitarCredencialAlternaCommand, Unit>
{
    private readonly ICredencialAlternaRepository _credencialesAlternas;
    private readonly IUnitOfWork _unitOfWork;

    public QuitarCredencialAlternaCommandHandler(
        ICredencialAlternaRepository credencialesAlternas,
        IUnitOfWork unitOfWork)
    {
        _credencialesAlternas = credencialesAlternas;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(QuitarCredencialAlternaCommand request, CancellationToken cancellationToken)
    {
        var existente = await _credencialesAlternas.ObtenerPorUsuarioIdAsync(request.UsuarioId, cancellationToken);

        if (existente != null)
        {
            _credencialesAlternas.Eliminar(existente);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return Unit.Value;
    }
}
