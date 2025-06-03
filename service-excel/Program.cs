var builder = WebApplication.CreateBuilder(args);

// Cargar appsettings.secret.json SOLO en entorno local (no en producción)
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("appsettings.secret.json", optional: false, reloadOnChange: true);
}

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient("BackendService", client => 
{
    client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("apiUrl")??builder.Configuration["apiUrl:url"]);
    client.Timeout = TimeSpan.FromMinutes(5); // Tiempo suficiente para archivos grandes
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
app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.MapControllers();
app.Run();