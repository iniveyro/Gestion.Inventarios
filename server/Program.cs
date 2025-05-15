using Microsoft.EntityFrameworkCore;
using server.Context.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Configuration.AddJsonFile("appsettings.secret.json", optional: false, reloadOnChange: true);

builder.Services.AddDbContext<DatabaseService>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("RailwayPostgresConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();