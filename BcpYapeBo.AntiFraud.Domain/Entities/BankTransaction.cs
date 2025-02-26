using BcpYapeBo.AntiFraud.Domain.Enums;
using BcpYapeBo.AntiFraud.Domain.ValueObjects;

namespace BcpYapeBo.AntiFraud.Domain.Entities
{
    public class BankTransaction
    {
        public Guid TransactionExternalId { get; set; }
        public AccountId SourceAccountId { get; set; }
        public AccountId TargetAccountId { get; set; }
        public BankTransactionType Type { get; set; }
        public TransactionValue Value { get; set; }
        public BankTransactionStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public string RejectionReason { get; set; }
        public int RetryCount { get; set; }

        // ENTITY FRAMEWORK, SERIALIZADORES, ETC
        public BankTransaction()
        {
        }
    }
}
