using System.Text;
using ApiAudiencia.Custom;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using server.Context.Database;
var builder = WebApplication.CreateBuilder(args);

// Cargar appsettings.secret.json SOLO en entorno local (no en producción)
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("appsettings.secret.json", optional: false, reloadOnChange: true);
}

// --- Configuración de JWT ---
// Usa la variable de entorno "JWT_KEY" si existe, si no, usa la de appsettings.secret.json
var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? builder.Configuration["JWT:key"];

// --- Configuración de la base de datos ---
// Usa la variable de entorno "ConnectionStrings__RailwayPostgresConnection" si existe, si no, usa la de appsettings.secret.json
var connectionString = Environment.GetEnvironmentVariable("PostgresConnection") 
    ?? builder.Configuration.GetConnectionString("RailwayPostgresConnection");

/*if (!builder.Environment.IsDevelopment()) 
{
    connectionString += ";SSL Mode=Require;Trust Server Certificate=true;";
}
*/
builder.Services.AddAuthorization(); // <-- Añade esta línea

// Configuración de autenticación JWT
builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!))
    };
});

// Configuración de la base de datos
builder.Services.AddDbContext<DatabaseService>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddSingleton<Utilidades>();

builder.Services.AddControllers(); // <-- Requerido para API Controllers
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var CorsPolicy = "AllowFrontend";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: CorsPolicy, policy =>
    {
        policy.WithOrigins(
            "https://audiencia-s.vercel.app/",
            "https://audiencia-l864aoahc-lautarosanche-gmailcoms-projects.vercel.app"
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });

    // Solo en desarrollo, permitir cualquier origen temporalmente
    if (builder.Environment.IsDevelopment())
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
    }
});

var app = builder.Build();
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Urls.Add($"http://*:{port}");

//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
//}

// Aplicar CORS
app.UseCors(builder.Environment.IsDevelopment() ? "AllowAll" : CorsPolicy);

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();