using MediatR;
using Touchliga.Application.DTOs;

namespace Touchliga.Application.Queries.Pago.GetCuentaCorriente;

public sealed record GetCuentaCorrienteQuery(long UsuarioId) : IRequest<CuentaCorrienteDto>;
