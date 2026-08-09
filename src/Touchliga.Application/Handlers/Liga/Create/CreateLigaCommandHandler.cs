using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.Commands.Liga.Create;
using Touchliga.Application.Common.Interfaces;
using DomainLiga = Touchliga.Domain.Entities.Liga;

namespace Touchliga.Application.Handlers.Liga.Create;

public sealed class CreateLigaCommandHandler : IRequestHandler<CreateLigaCommand, long>
{
    private readonly ILigaRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public CreateLigaCommandHandler(
        ILigaRepository repository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<long> Handle(
        CreateLigaCommand request,
        CancellationToken cancellationToken)
    {
        var liga = DomainLiga.Crear(
            request.Codigo,
            request.Nombre,
            request.Descripcion,
            request.Activo,
            _currentUser.UserId);

        await _repository.AgregarAsync(liga, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return liga.Id;
    }
}
