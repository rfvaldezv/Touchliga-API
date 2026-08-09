using MediatR;
using Touchliga.Application.Users.DTOs;

namespace Touchliga.Application.Users.Queries.GetMiPerfil;

public sealed record GetMiPerfilQuery() : IRequest<UsuarioAdminDto>;
