using EventBusRabbitMQ;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Ordering.API.Extentions;
using Ordering.API.RabbitMQ;
using Ordering.Application.Commands;
using Ordering.Application.Handlers;
using Ordering.Core.Repositories;
using Ordering.Core.Repositories.Base;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Repositories;
using Ordering.Infrastructure.Repositories.Base;
using RabbitMQ.Client;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration;

builder.Services.AddDbContext<OrderContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("OrderConnection")));

builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddTransient<IOrderRepository, OrderRepository>();


builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
    Assembly.GetExecutingAssembly(),
    typeof(CreateOrderCommand).Assembly));

builder.Services.AddSingleton<IRabbitMQConnection>(sp =>
{
    var factory = new ConnectionFactory()
    {
        HostName = Configuration["EventBus:HostName"],
    };

    if (!string.IsNullOrEmpty(Configuration["EventBus:UserName"]))
    {
        factory.UserName = Configuration["EventBus:UserName"];
    }

    if (!string.IsNullOrEmpty(Configuration["EventBus:Password"]))
    {
        factory.Password = Configuration["EventBus:Password"];
    }

    return new RabbitMQConnection(factory);
});

builder.Services.AddScoped<EventBusRabbitMQConsumer>();

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
builder.Services.AddSingleton(jwtSettings);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("DeliveryPolicy", policy => policy.RequireRole("Delivery"));
});



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

app.UseRabbitListener();

app.Run();
