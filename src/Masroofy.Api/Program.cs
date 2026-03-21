using Microsoft.EntityFrameworkCore;
using Masroofy.Api.Endpoints;
using Masroofy.Core.Data;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Register DbContext with Dependency Injection
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();
app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "Masroofy API"));

app.MapGet("/", () => "Hello World!");

app.MapExpenseEndpoints();

app.Run();
