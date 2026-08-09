using Touchliga.Domain.Entities;
using Touchliga.Domain.Interfaces;
using Touchliga.Persistence.Context;

namespace Touchliga.Persistence.Repositories;

/// <summary>
/// Implementación del repositorio de Liga.
/// </summary>
public sealed class LigaRepository
    : GenericRepository<Liga>, ILigaRepository
{
    public LigaRepository(TouchligaDbContext context) : base(context)
    {
    }
}
