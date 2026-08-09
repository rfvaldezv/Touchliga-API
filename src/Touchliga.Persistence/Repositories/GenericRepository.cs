using Touchliga.Domain.Interfaces;
using Touchliga.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Touchliga.Persistence.Repositories;

/// <summary>
/// Implementación base reutilizable de IGenericRepository&lt;T&gt; sobre
/// EF Core. Los repositorios de catálogo heredan de esta clase para no
/// repetir la misma implementación de Get/GetAll/Add/Update/Delete en
/// cada entidad.
/// </summary>
public class GenericRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : class
{
    protected readonly TouchligaDbContext Context;
    protected readonly DbSet<TEntity> DbSet;

    public GenericRepository(TouchligaDbContext context)
    {
        Context = context;
        DbSet = context.Set<TEntity>();
    }

    public virtual async Task<TEntity?> ObtenerPorIdAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        return await DbSet.FindAsync(new object[] { id }, cancellationToken);
    }

    public virtual async Task<IReadOnlyList<TEntity>> ObtenerTodosAsync(
        CancellationToken cancellationToken = default)
    {
        return await DbSet.AsNoTracking().ToListAsync(cancellationToken);
    }

    public virtual async Task AgregarAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
    }

    public virtual void Actualizar(TEntity entity)
    {
        DbSet.Update(entity);
    }

    public virtual void Eliminar(TEntity entity)
    {
        DbSet.Remove(entity);
    }
}
