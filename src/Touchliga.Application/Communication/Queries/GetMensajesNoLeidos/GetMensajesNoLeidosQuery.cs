using MediatR;

namespace Touchliga.Application.Communication.Queries.GetMensajesNoLeidos;

public sealed record GetMensajesNoLeidosQuery : IRequest<int>;
