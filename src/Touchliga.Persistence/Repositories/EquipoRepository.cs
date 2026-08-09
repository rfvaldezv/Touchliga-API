using Touchliga.Domain.Entities;
using Touchliga.Domain.Interfaces;
using Touchliga.Persistence.Context;

namespace Touchliga.Persistence.Repositories;

/// <summary>
/// Implementación del repositorio de Equipo.
/// </summary>
public sealed class EquipoRepository
    : GenericRepository<Equipo>, IEquipoRepository
{
    public EquipoRepository(TouchligaDbContext context) : base(context)
    {
    }
}
