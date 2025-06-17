using Gestion.Inventarios.Custom;
using server.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Configuración inicial
LoadSecretConfiguration(builder);
ConfigureServices(builder);

var app = builder.Build();
ConfigureMiddleware(app);

app.Run();

static void LoadSecretConfiguration(WebApplicationBuilder builder)
{
    if (builder.Environment.IsDevelopment())
    {
        builder.Configuration.AddJsonFile(
            "appsettings.secret.json", 
            optional: false, 
            reloadOnChange: true);
    }
}

static void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddAutoMapper(typeof(Program));
    
    builder.Services.AddCustomAuthentication(builder.Configuration);
    builder.Services.AddCustomDatabase(builder.Configuration);
    builder.Services.AddCustomHttpClients(builder.Configuration);
    builder.Services.AddCustomCors();
    
    builder.Services.AddSingleton<Utilidades>();
}

static void ConfigureMiddleware(WebApplication app)
{
    var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
    app.Urls.Add($"http://*:{port}");

    app.MapGet("/", () => "API Funcionando...");

    app.UseCustomSwagger();
    app.UseCors("AllowAll");
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseHttpsRedirection();
    app.MapControllers();
}