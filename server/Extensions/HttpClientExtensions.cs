namespace server.Extensions
{
    public static class HttpClientExtensions
    {
        public static IServiceCollection AddCustomHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient("ExcelService", client => 
            {
                client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("excel-service") ?? configuration["apiUrl:excel-service"]);
                client.Timeout = TimeSpan.FromMinutes(5);
            });
            return services;
        }
    }
}