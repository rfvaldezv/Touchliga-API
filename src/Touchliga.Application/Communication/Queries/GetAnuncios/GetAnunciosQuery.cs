using MediatR;
using Touchliga.Application.Communication.DTOs;

namespace Touchliga.Application.Communication.Queries.GetAnuncios;

public sealed record GetAnunciosQuery() : IRequest<IReadOnlyList<AnuncioDto>>;
