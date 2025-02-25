using BcpYapeBo.AntiFraud.Domain.Enums;

namespace BcpYapeBo.AntiFraud.Domain.Entities
{
    public class BankTransaction
    {
        public Guid TransactionId { get; set; }
        public Guid SourceAccountId { get; set; }
        public Guid DestinationAccountId { get; set; }
        public decimal Value { get; set; }
        public DateTime CreatedAt { get; set; }
        public BankTransactionStatus Status { get; set; }
    }
}
