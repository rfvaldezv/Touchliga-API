using Touchliga.Domain.Entities;
using Touchliga.Domain.Interfaces;
using Touchliga.Persistence.Context;

namespace Touchliga.Persistence.Repositories;

/// <summary>
/// Implementación del repositorio de Estado.
/// </summary>
public sealed class EstadoRepository
    : GenericRepository<Estado>, IEstadoRepository
{
    public EstadoRepository(TouchligaDbContext context) : base(context)
    {
    }
}
