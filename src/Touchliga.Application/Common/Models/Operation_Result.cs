namespace Touchliga.Application.Common.Models;

public sealed record OperationResult(
    bool Success,
    string Message);
