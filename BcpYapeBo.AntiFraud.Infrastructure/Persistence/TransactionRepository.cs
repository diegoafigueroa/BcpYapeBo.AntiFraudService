using BcpYapeBo.AntiFraud.Application.Ports.Driven;
using BcpYapeBo.AntiFraud.Domain.Enums;
using BcpYapeBo.AntiFraud.Domain.ValueObjects;
using BcpYapeBo.AntiFraud.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BcpYapeBo.AntiFraud.Infrastructure.Persistence
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly TransactionDbContext _context;

        public TransactionRepository(TransactionDbContext context)
        {
            _context = context;
        }

        public async Task<DailyTotalAmount> GetDailyAccumulatedAsync(Guid sourceAccountId, DateTime date)
        {
            return await _context.DailyAccumulated
                .Where(d => d.SourceAccountId == sourceAccountId && d.Accumulated.Date == date)
                .Select(d => d.Accumulated)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> HasRecentDuplicateTransaction(Guid sourceAccountId, Guid destinationAccountId, decimal value, DateTime timestamp)
        {
            // Por ejemplo, consideramos duplicado si hay transacción idéntica en los últimos 5 minutos
            var fiveMinutesAgo = timestamp.AddMinutes(-5);
            return await _context.Transactions.AnyAsync(t =>
                t.SourceAccountId == sourceAccountId &&
                t.DestinationAccountId == destinationAccountId &&
                t.Value == value &&
                t.CreatedAt >= fiveMinutesAgo);
        }

        public async Task<int> CountRejectedTransactionsInLastHour(Guid accountId)
        {
            var oneHourAgo = DateTime.UtcNow.AddHours(-1);
            return await _context.Transactions.CountAsync(t =>
                t.SourceAccountId == accountId &&
                t.Status == BankTransactionStatus.Rejected &&
                t.CreatedAt >= oneHourAgo);
        }
    }
}
