namespace ClearBank.DeveloperTest.Types
{
    public class MakePaymentResult
    {
        public MakePaymentResult(bool success)
        {
            Success = success;
        }

        public bool Success { get; }

        public static readonly MakePaymentResult Failure = new MakePaymentResult(false);
        public static readonly MakePaymentResult Ok = new MakePaymentResult(true);
    }
}
