using Microsoft.EntityFrameworkCore;
using Masroofy.Core;
using Masroofy.Core.Data;

namespace Masroofy.Tests.Integration;

public class AppDbContextTests
{
    [Fact]
    public async Task Can_Add_And_Retrieve_Expense()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        using var context = new AppDbContext(options);

        var expense = new Expense
        {
            Title = "Test Expense",
            Amount = 100m,
            Date = DateTime.UtcNow,
            Category = "Test",
            Notes = "Test Notes"
        };

        // Act
        context.Expenses.Add(expense);
        await context.SaveChangesAsync();

        // Assert
        var retrievedExpense = await context.Expenses.FindAsync(expense.Id);
        Assert.NotNull(retrievedExpense);
        Assert.Equal(expense.Title, retrievedExpense.Title);
        Assert.Equal(expense.Amount, retrievedExpense.Amount);
        Assert.Equal(expense.Date, retrievedExpense.Date);
        Assert.Equal(expense.Category, retrievedExpense.Category);
        Assert.Equal(expense.Notes, retrievedExpense.Notes);
    }

    [Fact]
    public async Task Cannot_Retrieve_NonExistent_Expense()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseInMemoryDatabase(databaseName: "TestDatabase")
    .Options;

        using var context = new AppDbContext(options);

        //Act
        var retrievedExpense = await context.Expenses.FindAsync(999);

        //Assert
        Assert.Null(retrievedExpense);
    }
}