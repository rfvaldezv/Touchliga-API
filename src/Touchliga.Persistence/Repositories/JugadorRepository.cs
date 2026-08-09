using Touchliga.Domain.Entities;
using Touchliga.Domain.Interfaces;
using Touchliga.Persistence.Context;

namespace Touchliga.Persistence.Repositories;

/// <summary>
/// Implementación del repositorio de Jugador.
/// </summary>
public sealed class JugadorRepository
    : GenericRepository<Jugador>, IJugadorRepository
{
    public JugadorRepository(TouchligaDbContext context) : base(context)
    {
    }
}
