using BcpYapeBo.AntiFraud.Domain.ValueObjects;

namespace BcpYapeBo.AntiFraud.Domain.Entities
{
    public class DailyAccumulated
    {
        public Guid SourceAccountId { get; set; }
        public DailyTotalAmount Accumulated { get; set; }
    }
}
