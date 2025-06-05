/**
 * InvoiceService Data Context - Entity Framework Core Database Context
 *
 * Purpose: Entity Framework Core database context for invoice data persistence
 * Features:
 * - Invoice entity configuration and database mapping
 * - Entity Framework Core DbContext with SQL Server provider
 * - Fluent API configuration for database schema definition
 * - Unique constraint enforcement on invoice numbers
 * - Precise decimal configuration for financial data
 *
 * Architecture: Part of the Persistence layer in clean architecture
 * - Provides database abstraction through Entity Framework Core
 * - Configures entity relationships and database constraints
 * - Supports repository pattern implementation
 * - Enables database migrations and schema management
 *
 * Database Configuration:
 * - Table: Invoices with GUID primary key
 * - Unique Index: InvoiceNumber for business rule enforcement
 * - Decimal Precision: decimal(18,2) for Amount field
 * - Enum Conversion: InvoiceStatus to integer mapping
 *
 * Author: Kim Hammerstad (with AI assistance from Claude 4)
 * Created: 2024 for Ventixe Event Management Platform
 */

using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Persistence.Contexts;

/// <summary>
/// Entity Framework Core database context for invoice management operations.
/// Provides database abstraction and configuration for invoice entities with
/// proper schema definition, constraints, and type mappings for SQL Server.
/// Supports the repository pattern and enables comprehensive data access operations.
/// </summary>
public class DataContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the DataContext class with the specified options.
    /// Options are typically configured through dependency injection in the application startup.
    /// </summary>
    /// <param name="options">The options to be used by the DbContext, including connection string and provider configuration</param>
    public DataContext(DbContextOptions<DataContext> options)
        : base(options) { }

    /// <summary>
    /// Gets or sets the DbSet for invoice entities in the database.
    /// Provides access to invoice data through Entity Framework Core's change tracking and query capabilities.
    /// Used by repositories for all CRUD operations on invoice data.
    /// </summary>
    public DbSet<InvoiceEntity> Invoices { get; set; } = null!;

    /// <summary>
    /// Configures the database schema, relationships, and constraints using Entity Framework Core's Fluent API.
    /// Called automatically by EF Core during context initialization and migration generation.
    /// Defines table structure, indexes, and data type mappings for proper database schema.
    /// </summary>
    /// <param name="modelBuilder">The builder used to construct the model for this context</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure InvoiceEntity with comprehensive database mapping
        modelBuilder.Entity<InvoiceEntity>(entity =>
        {
            // Map entity to Invoices table in the database
            entity.ToTable("Invoices");

            // Configure primary key with auto-generation
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            // Configure string properties with appropriate length constraints
            entity.Property(e => e.InvoiceNumber).IsRequired().HasMaxLength(50);
            entity.Property(e => e.EventName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.UserName).IsRequired().HasMaxLength(255);

            // Configure Amount with precise decimal type for financial accuracy
            // decimal(18,2) supports up to 16 digits before decimal point and 2 after
            entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");

            // Configure optional description with length constraint
            entity.Property(e => e.Description).HasMaxLength(1000);

            // Configure enum conversion to store as integer in database
            // Provides type safety in code while using efficient storage
            entity.Property(e => e.Status).HasConversion<int>();

            // Create unique index on InvoiceNumber to enforce business rule
            // Prevents duplicate invoice numbers which is critical for financial operations
            entity.HasIndex(e => e.InvoiceNumber).IsUnique();
        });
    }
}
