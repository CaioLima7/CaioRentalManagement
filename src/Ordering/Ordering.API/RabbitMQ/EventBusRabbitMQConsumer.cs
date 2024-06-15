using EventBusRabbitMQ.Common;
using EventBusRabbitMQ;
using MediatR;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using System.Text;
using AutoMapper;
using Ordering.Core.Repositories.Base;
using Ordering.Application.Commands;
using EventBusRabbitMQ.Events;
using Ordering.Core.Repositories;
using RabbitMQ.Client;

namespace Ordering.API.RabbitMQ
{
    public class EventBusRabbitMQConsumer
    {
        private readonly IRabbitMQConnection _connection;
        private readonly IServiceScopeFactory _scopeFactory;

        public EventBusRabbitMQConsumer(IRabbitMQConnection connection, IServiceScopeFactory scopeFactory)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
        }

        public void Consume()
        {
            var channel = _connection.CreateModel();

            Console.WriteLine("Consumer started");

            channel.ExchangeDeclare(exchange: "RentalExchange", type: ExchangeType.Direct, durable: true);

            channel.QueueDeclare(queue: EventBusConstants.BasketCheckoutQueue, durable: true, exclusive: false, autoDelete: false, arguments: null);

            channel.QueueBind(queue: EventBusConstants.BasketCheckoutQueue, exchange: "RentalExchange", routingKey: EventBusConstants.BasketCheckoutQueue);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                Console.WriteLine("Message received");
                Task.Run(async () => await ReceivedEvent(ea)).GetAwaiter().GetResult();
            };

            channel.BasicConsume(queue: EventBusConstants.BasketCheckoutQueue, autoAck: true, consumer: consumer);

            Console.WriteLine("Consumer ready");
        }

        private async Task ReceivedEvent(BasicDeliverEventArgs e)
        {
            Console.WriteLine("Processing message");

            if (e.RoutingKey == EventBusConstants.BasketCheckoutQueue)
            {
                using var scope = _scopeFactory.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

                var message = Encoding.UTF8.GetString(e.Body.Span);
                Console.WriteLine($"Message: {message}");

                try
                {
                    var rentalEvent = JsonConvert.DeserializeObject<RentalCheckoutEvent>(message);
                    Console.WriteLine("Message deserialized");

                    var command = mapper.Map<CreateOrderCommand>(rentalEvent);
                    Console.WriteLine("Command mapped");

                    await mediator.Send(command);
                    Console.WriteLine("Command sent");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"Unexpected routing key: {e.RoutingKey}");
            }
        }

        public void Disconnect()
        {
            _connection.Dispose();
        }
    }
}
