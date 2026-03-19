using Microsoft.EntityFrameworkCore;

using Masroofy.Core.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Register DbContext with Dependency Injection
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
