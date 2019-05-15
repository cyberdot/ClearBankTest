namespace ClearBank.DeveloperTest.Types
{
    public class Transaction
    {
        public Transaction(
            decimal amount, 
            TransactionType type)
        {
            Amount = amount;
            Type = type;
        }

        public decimal Amount { get; }
        public TransactionType Type { get; }
    }
}