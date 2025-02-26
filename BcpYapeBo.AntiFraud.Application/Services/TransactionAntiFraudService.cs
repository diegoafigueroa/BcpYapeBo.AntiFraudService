using BcpYapeBo.AntiFraud.Application.Ports.Driven;
using BcpYapeBo.AntiFraud.Application.Ports.Driving;
using BcpYapeBo.AntiFraud.Domain.Entities;
using BcpYapeBo.AntiFraud.Domain.Enums;
using BcpYapeBo.AntiFraud.Domain.ValueObjects;

namespace BcpYapeBo.AntiFraud.Application.Services
{
    public class TransactionAntiFraudService : ITransactionAntiFraudService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionAntiFraudRepository _antiFraudRepository;

        public TransactionAntiFraudService(
            ITransactionRepository transactionRepository,
            ITransactionAntiFraudRepository antiFraudRepository)
        {
            _transactionRepository = transactionRepository;
            _antiFraudRepository = antiFraudRepository;
        }

        public async Task<AntiFraudValidationResult> ValidateAsync(BankTransaction transactionEvent)
        {
            var today = transactionEvent.CreatedAt.Date;
            var dailyAccumulated = await _transactionRepository.GetDailyAccumulatedAsync(transactionEvent.SourceAccountId.Value, today);

            var validation = new AntiFraudValidationResult
            {
                TransactionExternalId = transactionEvent.TransactionExternalId
            };

            const decimal transactionLimit = 2000m;
            const decimal dailyLimit = 20000m;

            if (await _transactionRepository.HasRecentDuplicateTransaction(
                    transactionEvent.SourceAccountId.Value,
                    transactionEvent.TargetAccountId.Value,
                    transactionEvent.Value.Amount,
                    transactionEvent.CreatedAt))
            {
                validation.Status = BankTransactionStatus.Rejected;
                validation.RejectionReason = "Transacción duplicada detectada en los últimos 5 minutos.";
            }
            else if (await _transactionRepository.CountRejectedTransactionsInLastHour(transactionEvent.SourceAccountId.Value) >= 3)
            {
                validation.Status = BankTransactionStatus.Rejected;
                validation.RejectionReason = "Múltiples rechazos detectados en la última hora.";
            }
            else if (transactionEvent.Value.Amount > transactionLimit)
            {
                validation.Status = BankTransactionStatus.Rejected;
                validation.RejectionReason = $"El monto ({transactionEvent.Value}) excede el límite de {transactionLimit}";
            }
            else if (dailyAccumulated.ExceedsLimit(dailyLimit))
            {
                validation.Status = BankTransactionStatus.Rejected;
                validation.RejectionReason = $"El monto acumulado diario ({dailyAccumulated.Amount}) excede el límite de {dailyLimit}";
            }
            else
            {
                validation.Status = BankTransactionStatus.Approved;
            }

            // Guardamos el resultado de la validación para auditoría
            var validationHistory = new FraudCheckHistory
            {
                TransactionExternalId = transactionEvent.TransactionExternalId,
                SourceAccountId = transactionEvent.SourceAccountId.Value,
                Value = transactionEvent.Value.Amount,
                CreatedAt = transactionEvent.CreatedAt,
                Status = validation.Status,
                RejectionReason = validation.RejectionReason
            };

            await _antiFraudRepository.SaveValidationResultAsync(validationHistory);

            return validation;
        }
    }
}