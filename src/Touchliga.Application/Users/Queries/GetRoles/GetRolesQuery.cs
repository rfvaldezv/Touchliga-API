using MediatR;
using Touchliga.Application.Users.DTOs;

namespace Touchliga.Application.Users.Queries.GetRoles;

public sealed record GetRolesQuery() : IRequest<IReadOnlyList<RolDto>>;
