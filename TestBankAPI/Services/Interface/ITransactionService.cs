using TestBankAPI.Models;

namespace TestBankAPI.Services.Interface
{
    public interface ITransactionService
    {
        Response CreateNewTransaction(Transaction transaction);
        Response FindTransactionByDate(DateTime date);
        Response MakeDeposite(string AccountNumber, decimal Amout, string TransactionPin);
        Response MakeWithdrawal(string AccountNumber, decimal Amout, string TransactionPin);
        Response MakeFundsTransfer(string FromAccount, string ToAccount, decimal Amout, string TransactionPin);
    }
}
