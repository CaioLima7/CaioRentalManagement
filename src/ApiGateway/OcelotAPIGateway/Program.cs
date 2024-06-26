using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var webAppUrl = builder.Configuration["WebApp:url"];

builder.Services.AddCors(options =>
{
    options.AddPolicy("WebAppOrigin",
        builder =>
        {
            builder.WithOrigins(webAppUrl)
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]))
        };
    });

builder.Services.AddOcelot();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseCors("WebAppOrigin");

app.Use(async (context, next) =>
{
    if (context.Request.Method == "OPTIONS")
    {
        context.Response.StatusCode = 204;
        return;
    }

    await next();
});

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseOcelot().Wait();

app.Run();
