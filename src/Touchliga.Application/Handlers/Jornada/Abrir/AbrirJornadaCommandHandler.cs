using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Domain.Exceptions;
using Touchliga.Application.Commands.Jornada.Abrir;
using Touchliga.Application.Common.Interfaces;

namespace Touchliga.Application.Handlers.Jornada.Abrir;

public sealed class AbrirJornadaCommandHandler : IRequestHandler<AbrirJornadaCommand, Unit>
{
    private readonly IJornadaRepository _jornadas;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public AbrirJornadaCommandHandler(
        IJornadaRepository jornadas,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _jornadas = jornadas;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(AbrirJornadaCommand request, CancellationToken cancellationToken)
    {
        var jornada = await _jornadas.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Jornada");

        jornada.Abrir(_currentUser.UserId);
        _jornadas.Actualizar(jornada);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
