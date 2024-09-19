using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TravelGuide.DataTransferObjects.Accomodations.Input;
using TravelGuide.Services.Interfaces;

namespace TravelGuide.Services
{
    public class ScrapingBackgroundService : BackgroundService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IConnection connection;
        private readonly IModel channel;
        private const string QueueName = "scraping_tasks";

        public ScrapingBackgroundService(IServiceProvider serviceProvider, IConnectionFactory connectionFactory)
        {
            this.serviceProvider = serviceProvider;
            connection = connectionFactory.CreateConnection();
            channel = connection.CreateModel();
            channel.QueueDeclare(QueueName, durable: true, exclusive: false, autoDelete: false);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var searchRequest = JsonSerializer.Deserialize<HotelSearchRequestDto>(message);

                using (var scope = serviceProvider.CreateScope())
                {
                    var scraperService = scope.ServiceProvider.GetRequiredService<IAirbnbScraperService>();
                    var hotels = await scraperService.ScrapeHotelAsync(searchRequest.PriceMin, searchRequest.PriceMax);
                }

                channel.BasicAck(ea.DeliveryTag, multiple: false);
            };

            channel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);

            return Task.CompletedTask;
        }
    }
}