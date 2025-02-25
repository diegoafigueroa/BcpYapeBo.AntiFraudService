using BcpYapeBo.AntiFraud.Application.DTOs;
using BcpYapeBo.AntiFraud.Service.DTOs;

namespace BcpYapeBo.AntiFraud.Application.Ports.Driving
{
    public interface ITransactionAntiFraudService
    {
        Task<AntiFraudValidation> ValidateAsync(TransactionCreatedEvent transactionEvent);
    }
}