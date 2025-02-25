namespace BcpYapeBo.AntiFraud.Application.DTOs
{
    public record TransactionCreatedEvent(
        Guid TransactionExternalId,
        Guid SourceAccountId,
        Guid DestinationAccountId,
        decimal Value,
        DateTime CreatedAt
    );
}
