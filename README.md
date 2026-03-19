# Masroofy API

A learning-focused Expense Tracker API built with ASP.NET Core Minimal APIs.

## Purpose

This is a portfolio project designed to deeply learn:

- Entity Framework Core (Code-First, Migrations)
- Data Annotations Validation
- JWT Authentication with Refresh Token Rotation

## Tech Stack

- .NET 10 / ASP.NET Core
- Entity Framework Core + SQL Server
- xUnit for Testing
- Minimal APIs

## Project Structure

```text
Masroofy/
├── src/
│   ├── Masroofy.Api/        # API endpoints
│   └── Masroofy.Core/       # Entities, DbContext, DTOs
├── tests/
│   └── Masroofy.Tests/      # Unit & Integration tests
└── Directory.Build.props     # Shared build settings
```

## Domain Model

Currently, the primary domain models defined are:

- **Expense**: Tracks user expenditures with properties such as:
  - `Title`, `Amount`, `Date`, `Category`, `Notes`

## Current Progress

- ✅ Project structure established
- ✅ Core project set up
- ✅ `Expense` entity created
- ✅ Entity Framework Core & SQL Server packages installed
- ✅ Database context and migrations setup
- 🔲 Minimal API endpoints
- 🔲 Identity & JWT Authentication
