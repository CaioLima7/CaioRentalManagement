using Basket.API.Data.Interfaces;
using Basket.API.Data;
using Basket.API.Repositories;
using Basket.API.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Basket.API.Settings;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSingleton<IBasketDatabaseSettings>(sp =>
    sp.GetRequiredService<IOptions<BasketDatabaseSettings>>().Value);

builder.Services.Configure<BasketDatabaseSettings>(
    builder.Configuration.GetSection(nameof(BasketDatabaseSettings)));

builder.Services.AddTransient<IBasketRepository, BasketRepository>();
builder.Services.AddSingleton<IBasketContext, BasketContext>();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
