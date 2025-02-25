using BcpYapeBo.AntiFraud.Domain.Entities;
using BcpYapeBo.AntiFraud.Domain.ValueObjects;

namespace BcpYapeBo.AntiFraud.Application.Ports.Driven
{
    public interface ITransactionAntiFraudRepository
    {
        Task SaveValidationResultAsync(FraudCheckHistory validationHistory);
    }
}
