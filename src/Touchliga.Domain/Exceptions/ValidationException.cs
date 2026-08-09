namespace Touchliga.Domain.Exceptions;

public sealed class ValidationException : Exception
{
    public IReadOnlyList<string> Errors { get; }

    public ValidationException(IEnumerable<string> errors)
        : base("Se encontraron errores de validación.")
    {
        Errors = errors.ToList();
    }
}
