using BcpYapeBo.AntiFraud.Domain.Enums;

namespace BcpYapeBo.AntiFraud.Domain.Entities
{
    public class FraudCheckHistory
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid TransactionExternalId { get; set; }
        public Guid SourceAccountId { get; set; }
        public decimal Value { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public BankTransactionStatus Status { get; set; }
        public string? RejectionReason { get; set; }
    }
}
