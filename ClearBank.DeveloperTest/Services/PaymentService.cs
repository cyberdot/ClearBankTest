using Autofac.Features.Indexed;
using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Configuration;
using ClearBank.DeveloperTest.Validators;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IProvideAccountData accountDataStore;
        private readonly IIndex<PaymentScheme, IValidatePaymentRequest> paymentRequestValidators;


        public PaymentService(
            IConfiguration config,
            IIndex<DataStoreType, IProvideAccountData> accountDataStores,
            IIndex<PaymentScheme, IValidatePaymentRequest> paymentRequestValidators)
        {
            accountDataStore = accountDataStores[config.DataStoreType];
            this.paymentRequestValidators = paymentRequestValidators;
        }

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var account = accountDataStore.GetAccount(request.DebtorAccountNumber);
            
            var validator = paymentRequestValidators[request.PaymentScheme];
            var result = validator.Validate(account, request);
            
            if (result.Success)
            {
                var transaction = new Transaction(
                    request.Amount, 
                    TransactionType.Withdrawal);
                account.AddTransaction(transaction);
                accountDataStore.UpdateAccount(account);
            }

            return result;
        }
    }
}
