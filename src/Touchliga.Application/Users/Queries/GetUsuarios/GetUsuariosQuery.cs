using MediatR;
using Touchliga.Application.Users.DTOs;

namespace Touchliga.Application.Users.Queries.GetUsuarios;

public sealed record GetUsuariosQuery() : IRequest<IReadOnlyList<UsuarioAdminDto>>;
