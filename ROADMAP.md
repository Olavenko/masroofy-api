# Masroofy - Learning Roadmap

> **Project Goal:** A learning-focused Expense Tracker API built as a single Vertical Slice (Expense),
> designed to deeply understand Entity Framework Core, Validation, and Authentication in ASP.NET Core.
>
> **Structure:** 3 projects — `Masroofy.Api`, `Masroofy.Core`, `Masroofy.Tests`
>
> **How to use this roadmap:** Copy any phase section below and paste it to Claude.
> Claude will then provide step-by-step instructions **in Arabic** as if a Senior Developer
> is giving clear, specific tasks to a Junior Developer. You write the code yourself.

---

## Phase 0: Repository Setup & Solution Scaffold ✅

> **Paste this to Claude:**
> "We're working on Masroofy project, Phase 0: Repository Setup & Solution Scaffold.
> Give me the steps in Arabic like a Senior giving tasks to a Junior.
> I will do everything myself. Guide me step by step.
> Cover: creating the GitHub repo, cloning it, creating the solution and projects,
> setting up .gitignore, making the first commit, and pushing."

### What you will learn

- How to create a GitHub repository with proper settings
- How to clone a repo and work locally
- How to create a .NET solution with multiple projects and set up references
- Why we organize projects into `src/` and `tests/` folders
- What .gitignore does and why it matters for .NET projects
- How Directory.Build.props works and why we use it
- How to verify the solution builds before committing
- How to structure your first commit and push it to GitHub

### Folder Structure

```xml
Masroofy/
├── src/
│   ├── Masroofy.Api/
│   └── Masroofy.Core/
├── tests/
│   └── Masroofy.Tests/
├── Masroofy.slnx
├── Directory.Build.props
├── .gitignore
└── README.md
```

### Steps

1. Create a new GitHub repository (name: `masroofy-api`, public, with README, .gitignore for VisualStudio)

2. Clone the repository locally

```powershell
   cd D:\Projects
   git clone https://github.com/Olavenko/masroofy-api.git
   cd masroofy-api
```

1. Create the `src/` and `tests/` folders

```powershell
   mkdir src, tests
```

1. Create the solution file (`Masroofy.slnx`)

```powershell
   dotnet new sln --name Masroofy
```

1. Create the 3 projects inside their proper folders:

```powershell
   dotnet new web -n Masroofy.Api -o src/Masroofy.Api
   dotnet new classlib -n Masroofy.Core -o src/Masroofy.Core
   dotnet new xunit -n Masroofy.Tests -o tests/Masroofy.Tests
```

1. Add all projects to the solution

```powershell
   dotnet sln add src/Masroofy.Api src/Masroofy.Core tests/Masroofy.Tests
```

1. Add project references:

```powershell
   dotnet add src/Masroofy.Api reference src/Masroofy.Core
   dotnet add tests/Masroofy.Tests reference src/Masroofy.Core
   dotnet add tests/Masroofy.Tests reference src/Masroofy.Api
```

> **Dependencies:**
   >
   > - Api depends on Core (to access Entities and DbContext)
   > - Tests depends on Core + Api (to test both layers)
   > - Core depends on nothing (it's the innermost layer)

1. Create `Directory.Build.props` in the root with shared settings:

```xml
   <Project>
     <PropertyGroup>
       <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
       <Nullable>enable</Nullable>
       <ImplicitUsings>enable</ImplicitUsings>
     </PropertyGroup>
   </Project>
```

1. Verify the solution builds

```powershell
   dotnet build
```

1. Update the README.md with a brief project description

2. Stage, commit, and push the initial scaffold

```powershell
    git add .
    git status
    git commit -m "init: create solution with Api, Core, and Tests projects"
    git push
```

### Commits

- `init: create solution with Api, Core, and Tests projects`

---

## Phase 1: Data Layer (Entity + DbContext + Migration) ✅

> **Paste this to Claude:**
> "We're working on Masroofy project, Phase 1: Data Layer.
> Give me the steps in Arabic like a Senior giving tasks to a Junior.
> I will write the code myself. Guide me step by step."

### What you will learn

- How to create a clean Entity with proper data types
- Why we choose specific types (decimal for money, DateTime for dates)
- How DbContext works and what it does behind the scenes
- How EF Core migrations work and what they generate
- How to configure the database connection
- How to verify everything works before moving forward

### Entity: Expense

| Property | Type       | Notes                          |
|----------|------------|--------------------------------|
| Id       | int        | Primary key, auto-increment    |
| Title    | string     | Name of the expense            |
| Amount   | decimal    | Must be positive               |
| Date     | DateTime   | When the expense happened      |
| Category | string     | e.g., "Food", "Transport"      |
| Notes    | string?    | Optional notes                 |
| UserId   | string     | Added later in Phase 3 (Auth)  |

### Steps

1. Create the Expense entity in `src/Masroofy.Core/Entities/Expense.cs`
   > Use `decimal` for Amount (precise for money), `string?` for Notes (nullable),
   > and `= string.Empty` for required strings to avoid nullable warnings.

2. Install EF Core packages

```powershell
   cd src/Masroofy.Core
   dotnet add package Microsoft.EntityFrameworkCore.SqlServer
   dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore

   cd ../Masroofy.Api
   dotnet add package Microsoft.EntityFrameworkCore.Design

   dotnet tool install --global dotnet-ef
   # or: dotnet tool update --global dotnet-ef

   cd ../..
   dotnet build
```

1. Create the AppDbContext in `src/Masroofy.Core/Data/AppDbContext.cs`
   > Use Primary Constructor (C# 12), `DbSet<Expense>` via `=> Set<Expense>()`,
   > and override `OnModelCreating` to configure `Amount` with `.HasPrecision(18, 2)`
   > to avoid the decimal precision warning.

```cs
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
```

1. Configure the database connection string in `src/Masroofy.Api/appsettings.json`
   > Add `"ConnectionStrings"` section with `"DefaultConnection"` key.
   > Use `Server=localhost\SQLEXPRESS` (match your local SQL Server instance),
   > `Database=MasroofyDb`, `Integrated Security=True`, `TrustServerCertificate=True`.

2. Register DbContext in `src/Masroofy.Api/Program.cs`
   > Use `builder.Services.AddDbContext<AppDbContext>()` with
   > `options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))`.
   > Add `using` for `Masroofy.Core.Data` and `Microsoft.EntityFrameworkCore`.

3. Create and apply the first migration

```powershell
   dotnet ef migrations add InitialCreate --project src/Masroofy.Core --startup-project src/Masroofy.Api
   dotnet ef database update --project src/Masroofy.Core --startup-project src/Masroofy.Api
```

1. Verify the database is created with the correct table structure

```powershell
   sqlcmd -S localhost\SQLEXPRESS -d MasroofyDb -Q "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES"
```

   > Expected output: `__EFMigrationsHistory` and `Expenses` tables.

1. Write integration tests in `tests/Masroofy.Tests/Integration/AppDbContextTests.cs`

```powershell
   dotnet add tests/Masroofy.Tests package Microsoft.EntityFrameworkCore.InMemory
```

   > Delete `UnitTest1.cs`. Create two tests using `UseInMemoryDatabase`:
   >
   > - `Can_Add_And_Retrieve_Expense` — Add an Expense, SaveChanges, FindAsync by Id, assert all properties match.
   > - `Cannot_Retrieve_NonExistent_Expense` — Empty DB, FindAsync(999), assert result is null.
   >
```powershell
   dotnet test
```

1. Commit and push

```powershell
   git add .
   git commit -m "feat: add Expense entity, DbContext, and initial migration"
   git push
```

### Commits

- `feat: add Expense entity, DbContext, and initial migration`
- `test: add DbContext integration test`

---

## Phase 2: CRUD Endpoints + Deep Validation (Data Annotations)

> **Paste this to Claude:**
> "We're working on Masroofy project, Phase 2: CRUD Endpoints + Deep Validation.
> Give me the steps in Arabic like a Senior giving tasks to a Junior.
> I will write the code myself. Guide me step by step.
> Focus on teaching me Data Annotations deeply — what each one does,
> why we use it, and what happens if we skip it."

### What you will learn

- How Minimal API endpoints work (MapGet, MapPost, MapPut, MapDelete)
- How to create proper DTOs (request/response) and why we don't expose the Entity directly
- Data Annotations in depth: what each attribute does and why
- How validation works behind the scenes in ASP.NET Core
- How to return proper HTTP status codes (200, 201, 400, 404)
- How to test endpoints manually using Swagger
- How to write basic unit tests for validation

### DTOs

| DTO                | Purpose                        |
|--------------------|--------------------------------|
| CreateExpenseDto   | Request body for POST          |
| UpdateExpenseDto   | Request body for PUT           |
| ExpenseResponse    | Response body for GET          |

### Data Annotations to learn deeply

| Annotation           | Applied To             | Why                                      |
|----------------------|------------------------|------------------------------------------|
| [Required]           | Title, Category        | Prevent empty required fields            |
| [StringLength]       | Title, Category, Notes | Control min/max text length              |
| [Range]              | Amount                 | Ensure positive values only              |
| [DataType]           | Date                   | Enforce correct date format              |
| [RegularExpression]  | Category               | Allow only predefined categories         |

### Steps

1. Create the DTOs in Masroofy.Core (CreateExpenseDto, UpdateExpenseDto, ExpenseResponse)
2. Add Data Annotations to CreateExpenseDto with all validations
3. Add Data Annotations to UpdateExpenseDto with all validations
4. Create the POST /api/expenses endpoint with manual mapping and validation
5. Create the GET /api/expenses endpoint (get all)
6. Create the GET /api/expenses/{id} endpoint (get by id)
7. Create the PUT /api/expenses/{id} endpoint with validation
8. Create the DELETE /api/expenses/{id} endpoint
9. Test all endpoints with Swagger — test both valid and invalid data
10. Set up the test project with necessary packages (xUnit, FluentAssertions or similar)
11. Write unit tests for CreateExpenseDto validation (test each annotation: required, range, string length)
12. Write unit tests for UpdateExpenseDto validation
13. Write integration tests for CRUD endpoints (test success and failure scenarios)

### Commits

- `feat: add Expense DTOs with Data Annotations validation`
- `feat: add CRUD endpoints for Expense`
- `test: add validation unit tests`

---

## Phase 3: Authentication & Authorization (JWT + Refresh Tokens)

> **Paste this to Claude:**
> "We're working on Masroofy project, Phase 3: Auth (JWT + Refresh Tokens).
> Give me the steps in Arabic like a Senior giving tasks to a Junior.
> I will write the code myself. Guide me step by step.
> Explain WHY each step is needed and what would go wrong without it.
> Cover: Registration, Login, JWT, Refresh Token Rotation, Reuse Detection,
> Role-based Authorization, and protecting the Expense endpoints per user."

### What you will learn

- How ASP.NET Core Identity works and what it provides
- How JWT tokens work (header, payload, signature) and why we use them
- Why Access Tokens are short-lived and why we need Refresh Tokens
- How Refresh Token Rotation works and why it matters for security
- What Reuse Detection is and how it prevents token theft
- How Role-based Authorization works ([Authorize] with roles)
- How to link Expenses to Users so each user sees only their own data
- How to write tests for Auth logic

### Auth Entities

| Entity         | Purpose                                    |
|----------------|--------------------------------------------|
| ApplicationUser| Extends IdentityUser (no extra fields yet) |
| RefreshToken   | Stores token, expiry, revocation status    |

### Auth Endpoints

| Endpoint              | Method | Purpose                          |
|-----------------------|--------|----------------------------------|
| /api/auth/register    | POST   | Create new user account          |
| /api/auth/login       | POST   | Get access token + refresh token |
| /api/auth/refresh     | POST   | Rotate refresh token             |
| /api/auth/revoke      | POST   | Revoke a refresh token           |

### Steps

1. Install Identity and JWT packages
2. Create ApplicationUser in Masroofy.Core
3. Create RefreshToken entity in Masroofy.Core
4. Update AppDbContext to use IdentityDbContext and add RefreshToken DbSet
5. Create and apply the Identity migration
6. Configure Identity in Program.cs
7. Configure JWT authentication in Program.cs
8. Create Auth DTOs (RegisterDto, LoginDto, AuthResponse, RefreshDto)
9. Create the /api/auth/register endpoint
10. Create the /api/auth/login endpoint with JWT generation
11. Create the Refresh Token generation and storage logic
12. Create the /api/auth/refresh endpoint with token rotation
13. Implement Reuse Detection (revoke all tokens if reused token detected)
14. Create the /api/auth/revoke endpoint
15. Add UserId to Expense entity and create migration
16. Protect Expense endpoints with [Authorize]
17. Filter Expenses by the logged-in user (each user sees only their data)
18. Add Role-based Authorization (Admin can see all expenses)
19. Test the full Auth flow manually: register → login → access → refresh → revoke
20. Write unit tests for JWT token generation (verify claims, expiry)
21. Write unit tests for Refresh Token Rotation (verify old token is revoked, new token is created)
22. Write unit tests for Reuse Detection (verify all family tokens revoked on reuse)
23. Write integration tests for Auth endpoints (register, login, refresh, revoke)
24. Write integration tests for Expense authorization (user sees only their data, admin sees all)

### Commits

- `feat: add Identity setup and ApplicationUser`
- `feat: add JWT authentication configuration`
- `feat: add register and login endpoints`
- `feat: add refresh token rotation with reuse detection`
- `feat: add revoke endpoint`
- `feat: link Expenses to Users and add authorization`
- `feat: add role-based authorization (Admin role)`
- `test: add Auth unit tests`

---

## Final Checklist

> After completing all 3 phases, verify:

- [ ] All endpoints work correctly via Swagger
- [ ] Validation rejects invalid data with proper error messages
- [ ] Registration and Login work and return JWT tokens
- [ ] Refresh Token Rotation works correctly
- [ ] Reuse Detection revokes all tokens when a stolen token is used
- [ ] Each user can only see their own expenses
- [ ] Admin can see all expenses
- [ ] All tests pass
- [ ] All commits are clean and descriptive
- [ ] README.md is written explaining this is a learning project

### Final Commit: `docs: add README`
