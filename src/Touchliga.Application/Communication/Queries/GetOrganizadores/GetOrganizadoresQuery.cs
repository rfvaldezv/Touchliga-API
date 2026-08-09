using MediatR;
using Touchliga.Application.Communication.DTOs;

namespace Touchliga.Application.Communication.Queries.GetOrganizadores;

/// <summary>Usuarios con rol Administrador o Capturador — a quién le
/// puede escribir un participante normal si no conoce a nadie más.</summary>
public sealed record GetOrganizadoresQuery() : IRequest<IReadOnlyList<ContactoDto>>;
