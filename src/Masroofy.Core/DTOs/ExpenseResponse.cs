namespace Masroofy.Core.DTOs;

public record ExpenseResponse(
    int Id,
    string Title,
    decimal Amount,
    DateTime Date,
    string Category,
    string? Notes,
    DateTime CreatedAt,
    DateTime UpdatedAt
);