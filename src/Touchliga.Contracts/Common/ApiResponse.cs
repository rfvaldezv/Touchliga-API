public sealed class ApiResponse<T>
{
    public bool Success { get; init; }

    public string Message { get; init; } = "";

    public T? Data { get; init; }

    public IReadOnlyList<string>? Errors { get; init; }
}
