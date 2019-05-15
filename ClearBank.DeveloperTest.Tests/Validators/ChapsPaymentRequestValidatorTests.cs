using ClearBank.DeveloperTest.Tests.Builders;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validators;
using FluentAssertions;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Validators
{
    public class ChapsPaymentRequestValidatorTests
    {
        private readonly AccountBuilder builder;
        private readonly ChapsPaymentRequestValidator validator;

        public ChapsPaymentRequestValidatorTests()
        {
            builder = new AccountBuilder();
            validator = new ChapsPaymentRequestValidator();
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
        public void Should_fail_validation_if_payment_scheme_is_not_CHAPS()
        {
            var account = builder
                .WithPaymentSchemeAs(AllowedPaymentSchemes.Bacs)
                .Build();
            var request = new MakePaymentRequest();

            var result = validator.Validate(account, request);

            result.Success.Should().BeFalse();
        }

        [Fact]
        public void Should_fail_validation_if_account_status_is_not_live()
        {
            var account = builder
                .WithPaymentSchemeAs(AllowedPaymentSchemes.Chaps)
                .WithAccountStatusAs(AccountStatus.Disabled)
                .Build();
            var request = new MakePaymentRequest();

            var result = validator.Validate(account, request);

            result.Success.Should().BeFalse();
        }

        [Fact]
        public void Should_pass_validation_if_all_conditions_are_satisfied()
        {
            var account = builder
                .WithPaymentSchemeAs(AllowedPaymentSchemes.Chaps)
                .WithAccountStatusAs(AccountStatus.Live)
                .Build();
            var request = new MakePaymentRequest();

            var result = validator.Validate(account, request);

            result.Success.Should().BeTrue();
        }
    }
}