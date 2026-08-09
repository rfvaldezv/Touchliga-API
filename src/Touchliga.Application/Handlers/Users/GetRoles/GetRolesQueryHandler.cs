using MediatR;
using Touchliga.Domain.Interfaces;
using Touchliga.Application.Users.DTOs;
using Touchliga.Application.Users.Queries.GetRoles;

namespace Touchliga.Application.Handlers.Users.GetRoles;

public sealed class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, IReadOnlyList<RolDto>>
{
    private readonly IRolRepository _roles;

    public GetRolesQueryHandler(IRolRepository roles)
    {
        _roles = roles;
    }

    public async Task<IReadOnlyList<RolDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _roles.ObtenerTodosAsync();

        return roles.Select(r => new RolDto
        {
            Id = r.Id,
            Nombre = r.Nombre,
            Descripcion = r.Descripcion ?? string.Empty,
        }).ToList();
    }
}
