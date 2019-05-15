using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validators
{
    public interface IValidatePaymentRequest
    {
        MakePaymentResult Validate(Account account, MakePaymentRequest req);
    }
}