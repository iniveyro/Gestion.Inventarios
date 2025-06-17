using Microsoft.EntityFrameworkCore;
using server.Context.Database;

namespace server.Extensions
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection AddCustomDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = Environment.GetEnvironmentVariable("PostgresConnection") 
                ?? configuration.GetConnectionString("RailwayPostgresConnection");

            services.AddDbContext<DatabaseService>(options =>
                options.UseNpgsql(connectionString));
            return services;
        }
    }
}