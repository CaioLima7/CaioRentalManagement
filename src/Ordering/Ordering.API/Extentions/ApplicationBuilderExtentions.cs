using Ordering.API.RabbitMQ;

namespace Ordering.API.Extentions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseRabbitListener(this IApplicationBuilder app)
        {
            var life = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            life.ApplicationStarted.Register(() =>
            {
                var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
                using var scope = scopeFactory.CreateScope();
                var listener = scope.ServiceProvider.GetRequiredService<EventBusRabbitMQConsumer>();
                listener.Consume();
            });

            life.ApplicationStopping.Register(() =>
            {
                var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
                using var scope = scopeFactory.CreateScope();
                var listener = scope.ServiceProvider.GetRequiredService<EventBusRabbitMQConsumer>();
                listener.Disconnect();
            });

            return app;
        }
    }
}
