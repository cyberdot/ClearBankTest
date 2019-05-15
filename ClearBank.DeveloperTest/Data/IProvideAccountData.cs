using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Data
{
    public interface IProvideAccountData
    {
        Account GetAccount(string accountNumber);
        void UpdateAccount(Account account);
    }
}