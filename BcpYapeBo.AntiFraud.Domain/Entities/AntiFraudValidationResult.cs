using BcpYapeBo.AntiFraud.Domain.Enums;

namespace BcpYapeBo.AntiFraud.Domain.Entities
{
    public class AntiFraudValidationResult
    {
        public Guid TransactionExternalId { get; set; }
        public BankTransactionStatus Status { get; set; }
        public string RejectionReason { get; set; }
    }
}