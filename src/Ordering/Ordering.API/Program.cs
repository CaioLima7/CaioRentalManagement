using EventBusRabbitMQ;
using Microsoft.EntityFrameworkCore;
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
