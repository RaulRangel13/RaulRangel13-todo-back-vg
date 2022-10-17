using Identity.Infraestructure;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Api
{
    public class IdentityContextFactory : IDesignTimeDbContextFactory<IdentityDataContext>
    {
        public IdentityDataContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<IdentityDataContext>()
                .UseNpgsql("Server=localhost;Port=5432;Database=TodoList;User Id=postgres;Password=root", b => b.MigrationsAssembly("Infrastructure"));

            return new IdentityDataContext(builder.Options);
        }
    }
}
