using MediatR;
using Touchliga.Application.DTOs;

namespace Touchliga.Application.Queries.ConfiguracionSmtp.Get;

public sealed record GetConfiguracionSmtpQuery : IRequest<ConfiguracionSmtpDto?>;
