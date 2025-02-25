using BcpYapeBo.AntiFraud.Application.Ports.Driven;
using BcpYapeBo.AntiFraud.Service.DTOs;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace BcpYapeBo.AntiFraud.Infrastructure.Messaging
{
    public class AntiFraudValidationPublisher : IAntiFraudValidationPublisher
    {
        private readonly IProducer<Null, string> _producer;

        public AntiFraudValidationPublisher(IConfiguration configuration)
        {
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"] ?? "kafka:29092"
            };
            _producer = new ProducerBuilder<Null, string>(producerConfig).Build();
        }

        public async Task ProduceAsync(string topic, AntiFraudValidation message)
        {
            var jsonMessage = JsonSerializer.Serialize(message);
            await _producer.ProduceAsync(topic, new Message<Null, string> { Value = jsonMessage });
        }
    }
}
