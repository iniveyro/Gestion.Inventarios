namespace server.Extensions
{
    public static class SwaggerExtensions
    {
        public static WebApplication UseCustomSwagger(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = "swagger";
            });

            return app;
        }
    }
}