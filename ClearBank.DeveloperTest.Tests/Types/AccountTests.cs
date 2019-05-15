using ClearBank.DeveloperTest.Tests.Builders;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Types
{
    public class AccountTests
    {
        private readonly AccountBuilder builder;
        public AccountTests()
        {
            builder = new AccountBuilder();
        }

        [Fact]
        public void Should_increase_balance_when_new_deposit_transaction_is_added()
        {
            var account = builder
                .WithAccountNumberAs("123ABC")
                .WithAccountStatusAs(AccountStatus.Live)
                .WithBalanceAs(100)
                .Build();
            var transaction = new Transaction(
                100, 
                TransactionType.Deposit);
            
            account.AddTransaction(transaction);

            account.Balance.Should().Be(200);
        }

        [Fact]
        public void Should_decrease_balance_when_new_withdrawal_transaction_is_added()
        {
            var account = builder
                .WithAccountNumberAs("123ABC")
                .WithAccountStatusAs(AccountStatus.Live)
                .WithBalanceAs(300)
                .Build();
            var transaction = new Transaction(
                100, 
                TransactionType.Withdrawal);
            
            account.AddTransaction(transaction);

            account.Balance.Should().Be(200);
        }

        [Fact]
        public void Should_not_act_transaction_if_account_is_disabled()
        {
            const int originalBalance = 300;
            var account = builder
                .WithAccountNumberAs("123ABC")
                .WithAccountStatusAs(AccountStatus.Disabled)
                .WithBalanceAs(originalBalance)
                .Build();
            var transaction = new Transaction(
                100, 
                TransactionType.Withdrawal);
            
            account.AddTransaction(transaction);

            account.Balance.Should().Be(originalBalance);
        }
    }
}