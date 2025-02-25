using BcpYapeBo.AntiFraud.Domain.Enums;

namespace BcpYapeBo.AntiFraud.Service.DTOs
{
    public class AntiFraudValidation
    {
        public Guid TransactionExternalId { get; set; }
        public BankTransactionStatus Status { get; set; }
        public string? RejectionReason { get; set; }
    }
}