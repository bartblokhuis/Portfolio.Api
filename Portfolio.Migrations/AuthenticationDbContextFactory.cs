using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Portfolio.Database;

namespace Portfolio.Migrations
{
    public class AuthenticationDbContextFactory : IDesignTimeDbContextFactory<AuthenticationDbContext>
    {
        public AuthenticationDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("dataSettings.json")
                .Build();

            var dbContextBuilder = new DbContextOptionsBuilder<AuthenticationDbContext>();
            var connectionString = configuration["ConnectionString:DefaultConnection"];

            dbContextBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("Portfolio.Migrations"));

            return new AuthenticationDbContext(dbContextBuilder.Options);
        }
    }
}
