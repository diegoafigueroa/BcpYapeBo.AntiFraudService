using BcpYapeBo.AntiFraud.Application.DTOs;
using Confluent.Kafka;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using BcpYapeBo.AntiFraud.Application.Ports.Driving;
using BcpYapeBo.AntiFraud.Application.Ports.Driven;

namespace BcpYapeBo.AntiFraud.Infrastructure.Messaging
{
    public class AntiFraudTransactionConsumer : BackgroundService
    {
        private readonly ITransactionAntiFraudService _transactionAntiFraudService;
        private readonly IAntiFraudValidationPublisher _antiFraudValidationPublisher;
        private readonly IConsumer<Null, string> _consumer;

        public AntiFraudTransactionConsumer(
            ITransactionAntiFraudService transactionAntiFraudService,
            IAntiFraudValidationPublisher antiFraudValidationPublisher,
            IConfiguration configuration)
        {
            _transactionAntiFraudService = transactionAntiFraudService;
            _antiFraudValidationPublisher = antiFraudValidationPublisher;
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"] ?? "kafka:29092",
                GroupId = "anti-fraud-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _consumer = new ConsumerBuilder<Null, string>(consumerConfig).Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe("transaction-anti-fraud-service-validation");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = _consumer.Consume(stoppingToken);
                    var transactionEvent = JsonSerializer.Deserialize<TransactionCreatedEvent>(result.Message.Value);

                    // Validamos la transacción
                    var validation = await _transactionAntiFraudService.ValidateAsync(transactionEvent);

                    // Publicamos el resultado en Kafka
                    await _antiFraudValidationPublisher.ProduceAsync("transaction-anti-fraud-service-status-updated", validation);

                    _consumer.Commit(result);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error procesando mensaje: {ex.Message}");
                }
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _consumer.Close();
            return base.StopAsync(cancellationToken);
        }
    }
}