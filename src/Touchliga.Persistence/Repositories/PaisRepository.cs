using Touchliga.Domain.Entities;
using Touchliga.Domain.Interfaces;
using Touchliga.Persistence.Context;

namespace Touchliga.Persistence.Repositories;

/// <summary>
/// Implementación del repositorio de Pais.
/// </summary>
public sealed class PaisRepository
    : GenericRepository<Pais>, IPaisRepository
{
    public PaisRepository(TouchligaDbContext context) : base(context)
    {
    }
}
