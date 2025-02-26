using BcpYapeBo.AntiFraud.Application.Ports.Driven;
using BcpYapeBo.AntiFraud.Domain.Entities;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace BcpYapeBo.AntiFraud.Infrastructure.Messaging
{
    public class TransactionAntiFraudStatusPublisherKafka : IAntiFraudValidationPublisher
    {
        private readonly IProducer<Null, string> _kafkaProducer;

        public TransactionAntiFraudStatusPublisherKafka(IConfiguration configuration)
        {
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"] ?? "localhost:9092",
                Acks = Acks.All // CONFIRMATION FROM ALL REPLICAS
            };
            _kafkaProducer = new ProducerBuilder<Null, string>(producerConfig).Build();
        }

        public async Task ProduceAsync(string topic, AntiFraudValidationResult message)
        {
            var jsonMessage = JsonSerializer.Serialize(message);
            await _kafkaProducer.ProduceAsync(topic, new Message<Null, string> { Value = jsonMessage });

            var deliveryResult = await _kafkaProducer.ProduceAsync(topic, new Message<Null, string> { Value = jsonMessage });
            if (deliveryResult.Status != PersistenceStatus.Persisted)
            {
                throw new Exception($"Failed to send transaction to Kafka. Status: {deliveryResult.Status}");
            }
        }
    }
}
