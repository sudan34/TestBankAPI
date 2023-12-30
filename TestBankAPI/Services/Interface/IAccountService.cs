using TestBankAPI.Models;

namespace TestBankAPI.Services.Interface
{
    public interface IAccountService
    {
        Account Authentication(string AccountNumber, string Pin);
        IEnumerable<Account> GetAllAccounts();
        Account Create(Account account, string Pin, string ConfirmPin);
        void Update(Account account, string Pin = null);
        void Delete(int Id);
        Account GetById(int Id);
        Account GetByAccountNumber(string AccountNumber);

    }
}
