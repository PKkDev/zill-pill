using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ZillPillService.Infrastructure.Context
{
    public class AppDataBaseContextFactory : IDesignTimeDbContextFactory<AppDataBaseContext>
    {
        public AppDataBaseContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDataBaseContext>();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("connectionsettings.json", false, true);
            IConfigurationRoot config = builder.Build();

            string connectionString = config.GetConnectionString("AppConnectionString");

            if (string.IsNullOrEmpty(connectionString))
                throw new Exception("connection string not found");

            optionsBuilder.UseSqlServer(connectionString, opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds));
            return new AppDataBaseContext(optionsBuilder.Options);
        }
    }
}
