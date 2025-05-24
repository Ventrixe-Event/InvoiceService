using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Persistence.Contexts;

public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    public DataContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        optionsBuilder.UseSqlServer(
            "Server=tcp:ventixe-invoice-sqlserver.database.windows.net,1433;Initial Catalog=invoice_database;Persist Security Info=False;User ID=SqlAdmin;Password=Teigen88;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
        );

        return new DataContext(optionsBuilder.Options);
    }
}
