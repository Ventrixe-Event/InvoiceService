/**
 * InvoiceService Main Entry Point - Application Bootstrap and Configuration
 *
 * Purpose: Entry point and configuration for the InvoiceService API application
 * Features:
 * - Clean architecture dependency injection setup
 * - Entity Framework Core database configuration with SQL Server
 * - Swagger/OpenAPI documentation with unconditional exposure
 * - CORS configuration for cross-origin requests
 * - Repository and service pattern registration
 * - Authentication and authorization middleware
 *
 * Architecture: Presentation layer in clean architecture
 * - Entry point for the entire InvoiceService application
 * - Configures dependency injection container
 * - Sets up middleware pipeline for request processing
 * - Registers all services before builder.Build() as required
 *
 * Author: Kim Hammerstad (with AI assistance from Claude 4)
 * Created: 2024 for Ventixe Event Management Platform
 */

using Application.Interfaces;
using Application.Services;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using Persistence.Interfaces;
using Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container - all registrations must occur before builder.Build()

// Configure Web API controllers for RESTful endpoints
builder.Services.AddControllers();

// Configure CORS for cross-origin request handling
builder.Services.AddCors();

// Configure Entity Framework Core database context with SQL Server provider
// Connection string is retrieved from configuration (appsettings.json)
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"))
);

// Register repository layer dependencies for data access
// Uses scoped lifetime for per-request database context sharing
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();

// Register application layer dependencies for business logic
// Uses scoped lifetime to align with repository and database context
builder.Services.AddScoped<IInvoiceService, InvoiceService>();

// Configure OpenAPI/Swagger for API documentation
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

// Build the application after all service registrations are complete
var app = builder.Build();

// Configure CORS to allow cross-origin requests from any source
// Permissive settings for development - should be restricted in production
app.UseCors(x => x.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());

// Configure the HTTP request pipeline for processing incoming requests

// Swagger middleware is enabled unconditionally as per project requirements
// This ensures API documentation is always accessible for development and testing
app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Invoice Service API");
    c.RoutePrefix = string.Empty; // Makes Swagger UI available at root URL
});

// Standard ASP.NET Core middleware pipeline
app.UseHttpsRedirection(); // Redirect HTTP to HTTPS for security

// Authentication and authorization middleware for secure access
app.UseAuthentication(); // Enable authentication middleware
app.UseAuthorization(); // Enable authorization middleware

// Map controller endpoints for RESTful API routing
app.MapControllers();

// Start the application and begin listening for requests
app.Run();
