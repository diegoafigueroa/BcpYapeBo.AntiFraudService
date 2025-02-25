using BcpYapeBo.AntiFraud.Domain.ValueObjects;

namespace BcpYapeBo.AntiFraud.Application.Ports.Driven
{
    public interface ITransactionRepository
    {
        Task<DailyTotalAmount> GetDailyAccumulatedAsync(Guid sourceAccountId, DateTime date);
        Task<bool> HasRecentDuplicateTransaction(Guid sourceAccountId, Guid destinationAccountId, decimal value, DateTime timestamp);
        Task<int> CountRejectedTransactionsInLastHour(Guid accountId);
    }
}
