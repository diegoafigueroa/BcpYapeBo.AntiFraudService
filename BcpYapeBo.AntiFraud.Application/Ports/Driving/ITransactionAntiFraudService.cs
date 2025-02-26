using BcpYapeBo.AntiFraud.Domain.Entities;

namespace BcpYapeBo.AntiFraud.Application.Ports.Driving
{
    public interface ITransactionAntiFraudService
    {
        Task<AntiFraudValidationResult> ValidateAsync(BankTransaction transactionEvent);
    }
}