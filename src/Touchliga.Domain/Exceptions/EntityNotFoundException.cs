namespace Touchliga.Domain.Exceptions;

public sealed class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string entity)
        : base($"{entity} no fue encontrado.")
    {
    }
}
