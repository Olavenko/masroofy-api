using Microsoft.EntityFrameworkCore;

namespace Masroofy.Core.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Expense> Expenses => Set<Expense>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Expense>().Property(e => e.Amount)
            .HasPrecision(18, 2);
    }
}