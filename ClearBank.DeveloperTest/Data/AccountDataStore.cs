using System.Collections.Generic;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Data
{
    public class AccountDataStore : IProvideAccountData
    {
        public Account GetAccount(string accountNumber)
        {
            // Access database to retrieve account, code removed for brevity 
            //Database access is not implemented
            return new Account(
                "123123123ABC", 
                AccountStatus.Live, 
                AllowedPaymentSchemes.Bacs,
                new List<Transaction>());
        }

        public void UpdateAccount(Account account)
        {
            // Update account in database, code removed for brevity
            // Database access is not implemented
        }
    }
}
