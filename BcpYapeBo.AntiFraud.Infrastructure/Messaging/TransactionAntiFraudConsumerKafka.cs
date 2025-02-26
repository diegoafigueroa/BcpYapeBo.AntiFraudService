using Confluent.Kafka;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using BcpYapeBo.AntiFraud.Application.Ports.Driving;
using BcpYapeBo.AntiFraud.Application.Ports.Driven;
using Microsoft.Extensions.DependencyInjection;
using BcpYapeBo.AntiFraud.Domain.Entities;
using BcpYapeBo.AntiFraud.Domain.Enums;

namespace BcpYapeBo.AntiFraud.Infrastructure.Messaging
{
    public class TransactionAntiFraudConsumerKafka : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConsumer<Null, string> _consumer;

        public TransactionAntiFraudConsumerKafka(
            IServiceScopeFactory serviceScopeFactory,
            IConfiguration configuration)
        {
            _serviceScopeFactory = serviceScopeFactory;
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"] ?? "localhost:9092",
                GroupId = "anti-fraud-group",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };
            _consumer = new ConsumerBuilder<Null, string>(consumerConfig).Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Factory.StartNew(() => ConsumerLoop(stoppingToken), TaskCreationOptions.LongRunning);
        }
        private void ConsumerLoop(CancellationToken stoppingToken)
        {
            _consumer.Subscribe("transaction-anti-fraud-service-validation");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = _consumer.Consume(TimeSpan.FromMilliseconds(100));
                    var messageValue = result?.Message?.Value;

                    // SI NO HAY MENSAJE, CONTINUAMOS EL LOOP
                    if (string.IsNullOrEmpty(messageValue))
                        continue;

                    var bankTransaction = JsonSerializer.Deserialize<BankTransaction>(result.Message.Value);

                    if (bankTransaction != null && bankTransaction.TransactionExternalId != Guid.Empty)
                    {
                        using (var scope = _serviceScopeFactory.CreateScope())
                        {
                            var transactionAntiFraudService = scope.ServiceProvider.GetRequiredService<ITransactionAntiFraudService>();
                            var antiFraudValidationPublisher = scope.ServiceProvider.GetRequiredService<IAntiFraudValidationPublisher>();

                            // Validamos la transacción
                            var validation = transactionAntiFraudService.ValidateAsync(bankTransaction).Result;

                            // PUBLICAMOS EL RESULTADO EN KAFKA
                            antiFraudValidationPublisher.ProduceAsync("transaction-anti-fraud-service-status", validation);
                        }
                    }

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