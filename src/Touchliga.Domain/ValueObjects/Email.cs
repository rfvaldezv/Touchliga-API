using System.Text.RegularExpressions;
using Touchliga.Domain.Exceptions;

namespace Touchliga.Domain.ValueObjects;

public sealed record Email
{
    private static readonly Regex RegexEmail =
        new(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Email Create(string value)
    {
        value = value.Trim().ToLowerInvariant();

        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("El correo electrónico es obligatorio.");

        if (!RegexEmail.IsMatch(value))
            throw new DomainException("El correo electrónico no es válido.");

        return new Email(value);
    }

    public override string ToString()
    {
        return Value;
    }

    public static implicit operator string(Email email)
    {
        return email.Value;
    }
}
