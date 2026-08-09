using Touchliga.Domain.Entities;
using Touchliga.Domain.Interfaces;
using Touchliga.Persistence.Context;

namespace Touchliga.Persistence.Repositories;

/// <summary>
/// Implementación del repositorio de Cancha.
/// </summary>
public sealed class CanchaRepository
    : GenericRepository<Cancha>, ICanchaRepository
{
    public CanchaRepository(TouchligaDbContext context) : base(context)
    {
    }
}
