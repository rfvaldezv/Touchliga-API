using Touchliga.Domain.Entities;
using Touchliga.Domain.Interfaces;
using Touchliga.Persistence.Context;

namespace Touchliga.Persistence.Repositories;

/// <summary>
/// Implementación del repositorio de Categoria.
/// </summary>
public sealed class CategoriaRepository
    : GenericRepository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(TouchligaDbContext context) : base(context)
    {
    }
}
