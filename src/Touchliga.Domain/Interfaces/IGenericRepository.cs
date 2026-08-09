using Touchliga.Domain.Common;

namespace Touchliga.Domain.Interfaces;

/// <summary>
/// Operaciones CRUD comunes a los repositorios de catálogo
/// (entidades que heredan de BaseCatalogEntity). Los repositorios
/// específicos (ILigaRepository, IEquipoRepository, etc.) extienden
/// esta interfaz en vez de redeclarar los mismos métodos uno por uno.
/// </summary>
public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<TEntity?> ObtenerPorIdAsync(long id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default);

    Task AgregarAsync(TEntity entity, CancellationToken cancellationToken = default);

    void Actualizar(TEntity entity);

    void Eliminar(TEntity entity);
}
