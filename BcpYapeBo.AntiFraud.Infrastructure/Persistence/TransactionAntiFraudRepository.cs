using BcpYapeBo.AntiFraud.Application.Ports.Driven;
using BcpYapeBo.AntiFraud.Domain.Entities;
using BcpYapeBo.AntiFraud.Infrastructure.Persistence.Context;

namespace BcpYapeBo.AntiFraud.Infrastructure.Persistence
{
    public class TransactionAntiFraudRepository : ITransactionAntiFraudRepository
    {
        private readonly AntiFraudDbContext _context;

        public TransactionAntiFraudRepository(AntiFraudDbContext context)
        {
            _context = context;
        }

        public async Task SaveValidationResultAsync(FraudCheckHistory validationHistory)
        {
            _context.ValidationHistories.Add(validationHistory);
            await _context.SaveChangesAsync();
        }
    }
}
