using System.Data.SqlClient;
using Cartful.Service.Controllers;
using Microsoft.Extensions.Configuration;
using Cartful.Service.Entities;
using Cartful.Service.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Build a config object, using env vars and JSON providers.
IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build(); 

SqlSettings? sqlSettings = config.GetSection("SqlSettings").Get<SqlSettings>();

// Register the SqlConnection class with the container.
builder.Services.AddTransient<SqlConnection>(_ => new SqlConnection(sqlSettings?.connectionString));

// Register the class that depends on the SqlConnection with the container.
builder.Services.AddTransient<CartfulRepository>();


var app = builder.Build();

app.UseDefaultFiles(new DefaultFilesOptions { DefaultFileNames = new List<string> { "index.html" } });
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();
