using Touchliga.Domain.Entities;
using Touchliga.Domain.Interfaces;
using Touchliga.Persistence.Context;

namespace Touchliga.Persistence.Repositories;

/// <summary>
/// Implementación del repositorio de Ciudad.
/// </summary>
public sealed class CiudadRepository
    : GenericRepository<Ciudad>, ICiudadRepository
{
    public CiudadRepository(TouchligaDbContext context) : base(context)
    {
    }
}
