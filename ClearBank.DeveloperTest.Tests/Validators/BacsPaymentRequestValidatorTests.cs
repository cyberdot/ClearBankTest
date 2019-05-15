using ClearBank.DeveloperTest.Tests.Builders;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validators;
using FluentAssertions;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Validators
{
    public class BacsPaymentRequestValidatorTests
    {
        private readonly AccountBuilder builder;
        private readonly BacsPaymentRequestValidator validator;

        public BacsPaymentRequestValidatorTests()
        {
            builder = new AccountBuilder();

            validator = new BacsPaymentRequestValidator();
        }

        [Fact]
        public void Should_fail_validation_if_account_is_null()
        {
            var account = default(Account);
            var request = new MakePaymentRequest();

            var result =  validator.Validate(account, request);

            result.Success.Should().BeFalse();
        }

        [Fact]
        public void Should_fail_validation_if_payment_scheme_is_not_BACS()
        {
            var account = builder
                .WithPaymentSchemeAs(AllowedPaymentSchemes.Chaps)
                .Build();
            var request = new MakePaymentRequest();

            var result =  validator.Validate(account, request);

            result.Success.Should().BeFalse();
        }

        [Fact]
        public void Should_pass_validation_if_all_conditions_are_satisfied()
        {
            var account = builder
                .WithPaymentSchemeAs(AllowedPaymentSchemes.Bacs)
                .Build();
            var request = new MakePaymentRequest();

            var result =  validator.Validate(account, request);

            result.Success.Should().BeTrue();
        }
    }
}