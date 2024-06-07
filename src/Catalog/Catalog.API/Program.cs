using Catalog.API.Repositories.Interfaces;
using Catalog.API.Repositories;
using Npgsql;
using System.Data;
using Microsoft.Extensions.Options;
using Catalog.API.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<CatalogDatabaseSettings>(
    builder.Configuration.GetSection(nameof(CatalogDatabaseSettings)));

builder.Services.AddSingleton<ICatalogDatabaseSettings>(sp =>
    sp.GetRequiredService<IOptions<CatalogDatabaseSettings>>().Value);

builder.Services.AddTransient<IMotorcycleRepository, MotorcycleRepository>();

builder.Services.AddTransient<IDbConnection>(sp =>
    new NpgsqlConnection(builder.Configuration.GetSection(nameof(CatalogDatabaseSettings))
    ["ConnectionString"]));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
