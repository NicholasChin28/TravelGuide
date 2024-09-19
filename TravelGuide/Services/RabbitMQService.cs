using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RabbitMQ.Client;
using TravelGuide.DataTransferObjects.Accomodations.Input;
using TravelGuide.Services.Interfaces;

namespace TravelGuide.Services
{
    public class RabbitMQService : IRabbitMQService, IDisposable
    {
        private readonly IConnection connection;
        private readonly IModel channel;
        private const string QueueName = "scraping_tasks";

        public RabbitMQService(IConnectionFactory connectionFactory)
        {
            connection = connectionFactory.CreateConnection();
            channel = connection.CreateModel();
            channel.QueueDeclare(QueueName, durable: true, exclusive: false, autoDelete: false);
        }

        public void Dispose()
        {
            channel?.Dispose();
            connection?.Dispose();
        }

        public void PublishScrapingTask(HotelSearchRequestDto request)
        {
            var message = JsonSerializer.Serialize(request);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
                                routingKey: QueueName,
                                basicProperties: null,
                                body: body);
        }
    }
}