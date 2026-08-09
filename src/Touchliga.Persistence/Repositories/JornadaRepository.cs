using Touchliga.Domain.Entities;
using Touchliga.Domain.Interfaces;
using Touchliga.Persistence.Context;

namespace Touchliga.Persistence.Repositories;

/// <summary>
/// Implementación del repositorio de Jornada.
/// </summary>
public sealed class JornadaRepository
    : GenericRepository<Jornada>, IJornadaRepository
{
    public JornadaRepository(TouchligaDbContext context) : base(context)
    {
    }
}
