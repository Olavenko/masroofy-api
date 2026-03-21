namespace Masroofy.Core;

public class Expense
{
    public int Id { get; set; } // Primary key, auto-increment by convention
    public string Title { get; set; } = string.Empty; // Name of the expense
    public decimal Amount { get; set; } // Money amount - decimal is precise for financial data
    public DateTime Date { get; set; } // When the expense happened
    public string Category { get; set; } = string.Empty; // e.g., "Food", "Transport"
    public string? Notes { get; set; } // Optional - the ? means it can be null
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
