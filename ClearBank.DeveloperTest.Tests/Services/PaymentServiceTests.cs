using System.Collections.Generic;
using Autofac.Features.Indexed;
using ClearBank.DeveloperTest.Configuration;
using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Tests.Builders;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validators;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Services
{
    public class PaymentServiceTests
    {
        private readonly AccountBuilder accountBuilder;

        private readonly IConfiguration config;

        private readonly IProvideAccountData accountDataStore;
        private readonly IProvideAccountData backupAccountDataStore;

        private readonly IValidatePaymentRequest bacsValidator;
        private readonly IIndex<PaymentScheme, IValidatePaymentRequest> accountValidators;

        private readonly IIndex<DataStoreType, IProvideAccountData> accountDataStores;

        public PaymentServiceTests()
        {
            accountBuilder = new AccountBuilder();

            config = Substitute.For<IConfiguration>();
            
            accountDataStores = Substitute.For<IIndex<DataStoreType,IProvideAccountData>>();
            accountDataStore = Substitute.For<IProvideAccountData>();
            backupAccountDataStore = Substitute.For<IProvideAccountData>();
            accountDataStores[DataStoreType.Account].Returns(accountDataStore);
            accountDataStores[DataStoreType.BackupAccount].Returns(backupAccountDataStore);

            accountValidators = Substitute.For<IIndex<PaymentScheme,IValidatePaymentRequest>>();
            bacsValidator = Substitute.For<IValidatePaymentRequest>();
            accountValidators[PaymentScheme.Bacs].Returns(bacsValidator);
            bacsValidator.Validate(Arg.Any<Account>(), Arg.Any<MakePaymentRequest>())
                .Returns(MakePaymentResult.Failure);
        }


        [Fact]
        public void Should_get_debtor_account_by_its_account_number_using_configured_account_store()
        {
            config.DataStoreType.Returns(DataStoreType.BackupAccount);
            var request = new MakePaymentRequest
            {
                DebtorAccountNumber = "12312312ABC",
                PaymentScheme = PaymentScheme.Bacs
            };
            var paymentService = CreateService();

            paymentService.MakePayment(request);

            backupAccountDataStore.Received(1)
                .GetAccount(request.DebtorAccountNumber);
        }

        [Fact]
        public void Should_validate_account_and_payment_request()
        {
            const string accountNumber = "12312312ABC";
            var request = new MakePaymentRequest
            {
                PaymentScheme = PaymentScheme.Bacs,
                DebtorAccountNumber = accountNumber
            };
            var account = accountBuilder
                .WithAccountNumberAs(accountNumber)
                .Build();
            config.DataStoreType.Returns(DataStoreType.Account);
            accountDataStore.GetAccount(Arg.Any<string>()).Returns(account);
            var paymentService = CreateService();

            paymentService.MakePayment(request);

            bacsValidator.Received(1).Validate(account, request);
        }

        [Fact]
        public void Should_update_account_balance_if_validation_is_successful()
        {
            const string accountNumber = "12312312ABC";
            const int initialAccountBalance = 224;
            const int expectedAccountBalance = 102;
            var request = new MakePaymentRequest
            {
                PaymentScheme = PaymentScheme.Bacs,
                DebtorAccountNumber = accountNumber,
                Amount = 122
            };
            var account = accountBuilder
                .WithAccountNumberAs(accountNumber)
                .WithBalanceAs(initialAccountBalance)
                .Build();
            config.DataStoreType.Returns(DataStoreType.Account);
            accountDataStore.GetAccount(accountNumber).Returns(account);
            bacsValidator.Validate(account, request)
                .Returns(MakePaymentResult.Ok);
            var paymentService = CreateService();

            paymentService.MakePayment(request);

            account.Balance.Should().Be(expectedAccountBalance);
        }

        [Fact]
        public void Should_persist_account_with_updated_balance_if_validation_is_successful()
        {
            const string accountNumber = "12312312ABC";
            var request = new MakePaymentRequest
            {
                PaymentScheme = PaymentScheme.Bacs,
                DebtorAccountNumber = accountNumber,
                Amount = 122
            };
            var account = accountBuilder
                .WithAccountNumberAs(accountNumber)
                .WithBalanceAs(224)
                .Build();
            config.DataStoreType.Returns(DataStoreType.Account);
            accountDataStore.GetAccount(Arg.Any<string>()).Returns(account);
            bacsValidator.Validate(Arg.Any<Account>(), Arg.Any<MakePaymentRequest>())
                .Returns(MakePaymentResult.Ok);
            var paymentService = CreateService();

            paymentService.MakePayment(request);

            accountDataStore.Received(1).UpdateAccount(account);
        }

        [Fact]
        public void Should_not_update_balance_if_validation_fails()
        {
            const string accountNumber = "12312312ABC";
            const int accountBalance = 224;
            var request = new MakePaymentRequest
            {
                PaymentScheme = PaymentScheme.Bacs,
                DebtorAccountNumber = accountNumber,
                Amount = 122
            };
            var account = accountBuilder
                .WithAccountNumberAs(accountNumber)
                .WithBalanceAs(accountBalance)
                .Build();
            config.DataStoreType.Returns(DataStoreType.Account);
            accountDataStore.GetAccount(Arg.Any<string>()).Returns(account);
            bacsValidator.Validate(account, request)
                .Returns(MakePaymentResult.Failure);
            var paymentService = CreateService();

            paymentService.MakePayment(request);

            account.Balance.Should().Be(accountBalance);
            accountDataStore.DidNotReceive().UpdateAccount(account);
        }

        private PaymentService CreateService()
        {
            return new PaymentService(
                config,
                accountDataStores,
                accountValidators);
        }
    }
}