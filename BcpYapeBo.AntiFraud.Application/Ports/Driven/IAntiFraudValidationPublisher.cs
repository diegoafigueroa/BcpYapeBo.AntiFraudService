using BcpYapeBo.AntiFraud.Domain.Entities;

namespace BcpYapeBo.AntiFraud.Application.Ports.Driven
{
    public interface IAntiFraudValidationPublisher
    {
        Task ProduceAsync(string topic, AntiFraudValidationResult message);
    }
}
