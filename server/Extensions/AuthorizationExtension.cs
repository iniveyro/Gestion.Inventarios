namespace server.Extensions
{
    public static class AuthorizationExtension
    {
        public static IServiceCollection Authorization(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy =>
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(c =>
                            c.Type == "IsAdmin" && bool.Parse(c.Value) == true)));

                options.AddPolicy("AuthenticatedUser", policy =>
                    policy.RequireAuthenticatedUser());

                // Aquí puedes agregar más políticas para futuros roles
            });
            return services;
        }
    }
}