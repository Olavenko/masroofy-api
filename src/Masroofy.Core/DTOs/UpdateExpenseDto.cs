using System.ComponentModel.DataAnnotations;

namespace Masroofy.Core.DTOs;

public class UpdateExpenseDto
{
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Title must be between 2 and 100 characters.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Amount is required.")]
    [Range(0.01, 1_000_000, ErrorMessage = "Amount must be between 0.01 and 1,000,000.")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Date is required.")]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "Category is required.")]
    [RegularExpression("^(Food|Transport|Housing|Utilities|Entertainment|Other)$",
        ErrorMessage = "Category must be one of the following: Food, Transport, Housing, Utilities, Entertainment, Other.")]
    public string Category { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Notes must be at most 500 characters.")]
    public string? Notes { get; set; }
}
