using System.Collections.Generic;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Tests.Builders
{
    public class AccountBuilder
    {
        private string number;
        private AccountStatus status;
        private AllowedPaymentSchemes scheme;
        private List<Transaction> transactions;

        public AccountBuilder()
        {
            status = AccountStatus.Live;
            scheme = AllowedPaymentSchemes.Bacs;
            transactions = new List<Transaction>
            {
                new Transaction(123, TransactionType.Deposit)
            };
        }

        public AccountBuilder WithAccountNumberAs(string accNumber)
        {
            number = accNumber;
            return this;
        }

        public AccountBuilder WithBalanceAs(decimal newBalance)
        {
            transactions = new List<Transaction>
            {
                new Transaction(newBalance, TransactionType.Deposit)
            };
            return this;
        }

        public AccountBuilder WithPaymentSchemeAs(AllowedPaymentSchemes newScheme)
        {
            scheme = newScheme;
            return this;
        }

        public AccountBuilder WithAccountStatusAs(AccountStatus newStatus)
        {
            status = newStatus;
            return this;
        }

        public Account Build()
        {
            return new Account(
                number,
                status,
                scheme,
                transactions);
        }
    }
}