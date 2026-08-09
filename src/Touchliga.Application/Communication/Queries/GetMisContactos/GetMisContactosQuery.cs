using MediatR;
using Touchliga.Application.Communication.DTOs;

namespace Touchliga.Application.Communication.Queries.GetMisContactos;

public sealed record GetMisContactosQuery() : IRequest<IReadOnlyList<ContactoDto>>;
