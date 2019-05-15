using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validators
{
    public class FasterPaymentsRequestValidator : IValidatePaymentRequest
    {
        public MakePaymentResult Validate(Account account, MakePaymentRequest req)
        {
            if (account == null)
            {
                return MakePaymentResult.Failure;
            }

            //NOTE: Enum.HasFlag used to incur performance penalty,
            // this has been fixed in Roslyn and Core CLR
            if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments))
            {
                return MakePaymentResult.Failure;
            }

            if (account.Balance < req.Amount)
            {
                return MakePaymentResult.Failure;
            }

            return MakePaymentResult.Ok;
        }
    }
}