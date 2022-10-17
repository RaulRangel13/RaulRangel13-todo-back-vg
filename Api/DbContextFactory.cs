using Identity.Infraestructure;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Api
{
    public class DbContextFactory : IDesignTimeDbContextFactory<TodoContext>
    {
        public TodoContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<TodoContext>()
                .UseNpgsql("Server=localhost;Port=5432;Database=TodoList;User Id=postgres;Password=root", b => b.MigrationsAssembly("Infrastructure"));

            return new TodoContext(builder.Options);
        }
    }
}
