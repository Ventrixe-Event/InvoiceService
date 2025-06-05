/**
 * InvoiceService Data Context Factory - Design-Time DbContext Factory
 *
 * Purpose: Design-time factory for creating DataContext instances during EF Core tooling operations
 * Features:
 * - Enables Entity Framework Core CLI tools (dotnet ef) functionality
 * - Provides database context for migrations, database updates, and scaffolding
 * - Configures connection string for design-time operations
 * - Supports development and deployment database operations
 *
 * Architecture: Part of the Persistence layer in clean architecture
 * - Implements IDesignTimeDbContextFactory<T> for EF Core tooling
 * - Provides DbContext instances without dependency injection
 * - Enables database schema management and migration generation
 * - Supports Entity Framework Core command-line operations
 *
 * Usage: Used automatically by EF Core tools such as:
 * - dotnet ef migrations add [MigrationName]
 * - dotnet ef database update
 * - dotnet ef dbcontext scaffold
 *
 * Author: Kim Hammerstad (with AI assistance from Claude 4)
 * Created: 2024 for Ventixe Event Management Platform
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Persistence.Contexts;

/// <summary>
/// Design-time factory for creating DataContext instances during Entity Framework Core tooling operations.
/// Implements the factory pattern required by EF Core CLI tools for migrations and database operations.
/// Provides a configured DataContext instance without relying on dependency injection or application startup.
/// Essential for development workflows involving database schema changes and migrations.
/// </summary>
public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    /// <summary>
    /// Creates a DataContext instance for design-time operations such as migrations and database updates.
    /// Called automatically by Entity Framework Core tools (dotnet ef) when performing database operations.
    /// Configures the context with a direct connection string for SQL Server database access.
    /// </summary>
    /// <param name="args">Command-line arguments passed from EF Core tools (typically not used)</param>
    /// <returns>A configured DataContext instance ready for database operations</returns>
    public DataContext CreateDbContext(string[] args)
    {
        // Create DbContextOptionsBuilder for SQL Server configuration
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();

        // Configure SQL Server connection string for Azure SQL Database
        // Connection string includes server, database, authentication, and security settings
        // Used for development and migration operations against the invoice database
        optionsBuilder.UseSqlServer(
            "Server=tcp:ventixe-invoice-sqlserver.database.windows.net,1433;Initial Catalog=invoice_database;Persist Security Info=False;User ID=SqlAdmin;Password=Teigen88;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
        );

        // Return configured DataContext instance for EF Core tooling use
        return new DataContext(optionsBuilder.Options);
    }
}
