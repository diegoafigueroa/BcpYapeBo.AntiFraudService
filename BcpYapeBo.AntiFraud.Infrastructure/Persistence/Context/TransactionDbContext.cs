using BcpYapeBo.AntiFraud.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BcpYapeBo.AntiFraud.Infrastructure.Persistence.Context
{
    public class TransactionDbContext : DbContext
    {
        public DbSet<DailyAccumulated> DailyAccumulated { get; set; }
        public DbSet<BankTransaction> Transactions { get; set; }

        public TransactionDbContext(DbContextOptions<TransactionDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DailyAccumulated>(entity =>
            {
                entity.HasKey(d => new { d.SourceAccountId, d.Accumulated.Date });
                entity.OwnsOne(d => d.Accumulated, a =>
                {
                    a.Property(x => x.Amount).HasColumnName("Amount");
                    a.Property(x => x.Date).HasColumnName("Date");
                });
            });

            // Configuración de Transactions  
            modelBuilder.Entity<BankTransaction>(entity =>
            {
                entity.HasKey(t => t.TransactionId);
                entity.Property(t => t.Value).HasColumnType("decimal(18,2)");
                entity.Property(t => t.CreatedAt).IsRequired();
            });
        }
    }
}
