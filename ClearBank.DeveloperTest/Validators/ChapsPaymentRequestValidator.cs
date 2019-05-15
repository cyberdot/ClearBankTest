using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validators
{
    public class ChapsPaymentRequestValidator : IValidatePaymentRequest
    {
        public MakePaymentResult Validate(Account account, MakePaymentRequest req)
        {
            if (account == null)
            {
                return MakePaymentResult.Failure;
            }

            //NOTE: Enum.HasFlag used to have performance penalty,
            // this has been fixed in Roslyn and Core CLR
            if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps))
            {
                return MakePaymentResult.Failure;
            }

            if (account.Status != AccountStatus.Live)
            {
                return MakePaymentResult.Failure;
            }

            return MakePaymentResult.Ok;
        }
    }
}