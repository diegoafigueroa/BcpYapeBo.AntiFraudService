using BcpYapeBo.AntiFraud.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BcpYapeBo.AntiFraud.Infrastructure.Persistence.Context
{
    public class AntiFraudDbContext : DbContext
    {
        public DbSet<FraudCheckHistory> ValidationHistories { get; set; }

        public AntiFraudDbContext(DbContextOptions<AntiFraudDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FraudCheckHistory>(entity =>
            {
                entity.HasKey(v => v.Id);
                entity.Property(v => v.TransactionExternalId).IsRequired();
                entity.Property(v => v.SourceAccountId).IsRequired();
                entity.Property(v => v.Value).IsRequired();
                entity.Property(v => v.CreatedAt).IsRequired();
                entity.Property(v => v.Status).IsRequired();
                entity.Property(v => v.RejectionReason);
            });
        }
    }
}
