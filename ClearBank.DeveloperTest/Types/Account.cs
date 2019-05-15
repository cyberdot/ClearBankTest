using System.Collections.Generic;
using System.Linq;

namespace ClearBank.DeveloperTest.Types
{
    public class Account
    {
        private readonly List<Transaction> transactions;

        public Account(
            string accountNumber,
            AccountStatus status,
            AllowedPaymentSchemes scheme,
            List<Transaction> transactions)
        {
            AccountNumber = accountNumber;
            Status = status;
            AllowedPaymentSchemes = scheme;
            this.transactions = transactions ?? new List<Transaction>();
        }

        public string AccountNumber { get; }

        public decimal Balance
        {
            get
            {
                var deposits = transactions
                    .Where(t => t.Type == TransactionType.Deposit)
                    .Sum(t => t.Amount);
                var withdrawals = transactions
                    .Where(t => t.Type == TransactionType.Withdrawal)
                    .Sum(t => t.Amount);
                return deposits - withdrawals;
            }
        }

        public AccountStatus Status { get; }
        public AllowedPaymentSchemes AllowedPaymentSchemes { get; }

        public void AddTransaction(Transaction t)
        {
            if (Status != AccountStatus.Disabled)
            {
                transactions.Add(t);
            }
        }
    }
}
