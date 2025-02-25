using BcpYapeBo.AntiFraud.Domain.Entities;
using BcpYapeBo.AntiFraud.Service.DTOs;

namespace BcpYapeBo.AntiFraud.Application.Ports.Driven
{
    public interface IAntiFraudValidationPublisher
    {
        Task ProduceAsync(string topic, AntiFraudValidation message);
    }
}
