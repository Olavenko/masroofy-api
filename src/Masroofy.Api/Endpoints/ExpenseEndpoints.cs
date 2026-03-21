using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

using Masroofy.Core;
using Masroofy.Core.Data;
using Masroofy.Core.DTOs;

namespace Masroofy.Api.Endpoints;

public static class ExpenseEndpoints
{
    public static IEndpointRouteBuilder MapExpenseEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/expenses", async (CreateExpenseDto request, AppDbContext context) =>
        {
            // Step 1: Manual validation (Minimal APIs don't auto-validate)
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(
                request,
                new ValidationContext(request),
                validationResults,
                validateAllProperties: true
            );

            if (!isValid)
            {
                var errors = validationResults
                    .Where(v => v.MemberNames.Any())
                    .GroupBy(v => v.MemberNames.First())
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(v => v.ErrorMessage ?? "Invalid value").ToArray()
                    );

                return Results.ValidationProblem(errors);
            }

            // Step 2: Map DTO to Entity
            var expense = new Expense
            {
                Title = request.Title,
                Amount = request.Amount,
                Date = request.Date,
                Category = request.Category,
                Notes = request.Notes,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Step 3: Save to database
            context.Expenses.Add(expense);
            await context.SaveChangesAsync();

            // Step 4: Map Entity to Response
            var response = new ExpenseResponse(
                expense.Id,
                expense.Title,
                expense.Amount,
                expense.Date,
                expense.Category,
                expense.Notes,
                expense.CreatedAt,
                expense.UpdatedAt
            );

            // Step 5: Return 201 Created with location header
            return Results.Created($"/api/expenses/{expense.Id}", response);
        });

        app.MapGet("/api/expenses", async (AppDbContext context) =>
        {
            var expenses = await context.Expenses.ToListAsync();

            var response = expenses.Select(expense => new ExpenseResponse(
                expense.Id,
                expense.Title,
                expense.Amount,
                expense.Date,
                expense.Category,
                expense.Notes,
                expense.CreatedAt,
                expense.UpdatedAt
            ));
            return Results.Ok(response);
        });

        app.MapGet("/api/expenses/{id}", async (int id, AppDbContext context) =>
        {
            var expense = await context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return Results.NotFound();
            }
            var response = new ExpenseResponse(
                expense.Id,
                expense.Title,
                expense.Amount,
                expense.Date,
                expense.Category,
                expense.Notes,
                expense.CreatedAt,
                expense.UpdatedAt
            );
            return Results.Ok(response);
        });

        app.MapPut("/api/expenses/{id}", async (int id, AppDbContext context, UpdateExpenseDto request) =>
        {
            var validationResults = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(
                request,
                new ValidationContext(request),
                validationResults,
                validateAllProperties: true
            );

            if (!isValid)
            {
                var errors = validationResults
                    .Where(v => v.MemberNames.Any())
                    .GroupBy(v => v.MemberNames.First())
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(v => v.ErrorMessage ?? "Invalid value").ToArray()
                    );

                return Results.ValidationProblem(errors);
            }

            var expense = await context.Expenses.FindAsync(id);

            if (expense == null)
            {
                return Results.NotFound();
            }

            expense.Title = request.Title;
            expense.Amount = request.Amount;
            expense.Date = request.Date;
            expense.Category = request.Category;
            expense.Notes = request.Notes;
            expense.UpdatedAt = DateTime.UtcNow;

            await context.SaveChangesAsync();

            var response = new ExpenseResponse(
                expense.Id,
                expense.Title,
                expense.Amount,
                expense.Date,
                expense.Category,
                expense.Notes,
                expense.CreatedAt,
                expense.UpdatedAt
            );

            return Results.Ok(response);
        });

        app.MapDelete("/api/expenses/{id}", async (int id, AppDbContext context) =>
        {
            var expense = await context.Expenses.FindAsync(id);

            if (expense == null)
            {
                return Results.NotFound();
            }

            context.Expenses.Remove(expense);
            await context.SaveChangesAsync();

            return Results.NoContent();

        });

        return app;
    }
}