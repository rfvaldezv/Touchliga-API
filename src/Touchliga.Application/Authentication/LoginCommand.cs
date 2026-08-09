using Touchliga.Application.Authentication.DTOs;
using MediatR;

namespace Touchliga.Application.Authentication.Commands.Login;

public sealed record LoginCommand(

    string Correo,

    string Password

) : IRequest<LoginResponse>;
