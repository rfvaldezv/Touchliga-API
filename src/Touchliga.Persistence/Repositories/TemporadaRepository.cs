using Touchliga.Domain.Entities;
using Touchliga.Domain.Interfaces;
using Touchliga.Persistence.Context;

namespace Touchliga.Persistence.Repositories;

/// <summary>
/// Implementación del repositorio de Temporada.
/// </summary>
public sealed class TemporadaRepository
    : GenericRepository<Temporada>, ITemporadaRepository
{
    public TemporadaRepository(TouchligaDbContext context) : base(context)
    {
    }
}
