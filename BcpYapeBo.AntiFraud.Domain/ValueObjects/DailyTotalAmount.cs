namespace BcpYapeBo.AntiFraud.Domain.ValueObjects
{
    public class DailyTotalAmount
    {
        public decimal Amount { get; private set; }
        public DateTime Date { get; private set; }

        public DailyTotalAmount(decimal amount, DateTime date)
        {
            if (amount < 0)
                throw new ArgumentException("El monto acumulado no puede ser negativo");

            Amount = amount;
            Date = date.Date; // Normalizamos la fecha a solo el día
        }

        public bool ExceedsLimit(decimal limit)
        {
            return Amount > limit;
        }
    }
}