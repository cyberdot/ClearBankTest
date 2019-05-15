using ClearBank.DeveloperTest.Tests.Builders;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validators;
using FluentAssertions;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Validators
{
    public class FasterPaymentsRequestValidatorTests
    {
        private readonly AccountBuilder builder;
        private readonly FasterPaymentsRequestValidator validator;

        public FasterPaymentsRequestValidatorTests()
        {
            builder = new AccountBuilder();
            validator = new FasterPaymentsRequestValidator();
        }

        [Fact]
        public void Should_fail_validation_if_account_is_null()
        {
            var account = default(Account);
            var request = new MakePaymentRequest();

            var result = validator.Validate(account, request);

            result.Success.Should().BeFalse();
        }

        [Fact]
        public void Should_fail_validation_if_payment_scheme_is_not_of_faster_payments_type()
        {
            var account = builder
                .WithPaymentSchemeAs(AllowedPaymentSchemes.Bacs)
                .Build();
            var request = new MakePaymentRequest();

            var result = validator.Validate(account, request);

            result.Success.Should().BeFalse();
        }

        [Fact]
        public void Should_fail_validation_if_account_balance_is_less_than_requested_payment_amount()
        {
            var account = builder
                .WithPaymentSchemeAs(AllowedPaymentSchemes.FasterPayments)
                .WithBalanceAs(2)
                .Build();
            var request = new MakePaymentRequest
            {
                PaymentScheme = PaymentScheme.FasterPayments,
                Amount = 100
            };

            var result = validator.Validate(account, request);

            result.Success.Should().BeFalse();
        }

        [Fact]
        public void Should_pass_validation_if_all_conditions_are_satisfied()
        {
            var account = builder
                .WithPaymentSchemeAs(AllowedPaymentSchemes.FasterPayments)
                .WithBalanceAs(200)
                .Build();
            var request = new MakePaymentRequest
            {
                PaymentScheme = PaymentScheme.FasterPayments,
                Amount = 100
            };

            var result = validator.Validate(account, request);

            result.Success.Should().BeTrue();
        }
    }
}