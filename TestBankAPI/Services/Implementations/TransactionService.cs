using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TestBankAPI.DAL;
using TestBankAPI.Models;
using TestBankAPI.Services.Interface;
using TestBankAPI.Utils;

namespace TestBankAPI.Services.Implementations
{
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationDbContext _context;
        ILogger<TransactionService> _logger;
        private AppSettings _setting;
        private static string _ourBankSettlementAccount;
        private readonly IAccountService _accountService;

        public TransactionService(ApplicationDbContext context, ILogger<TransactionService> logger, IOptions<AppSettings> setting, IAccountService accountService)
        {
            _context = context;
            _logger = logger;
            _setting = setting.Value;
            _ourBankSettlementAccount = _setting.OurBankSettlementAccount;
            _accountService = accountService;
        }

        public Response CreateNewTransaction(Transaction transaction)
        {
            Response response = new Response();
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
            response.ResonseCode = "00";
            response.ResponseMessage = "Transaction created successfully!";
            response.Data = null;
            return response;
        }

        public Response FindTransactionByDate(DateTime date)
        {
            Response response = new Response();
            var transaction = _context.Transactions.Where(x => x.TransactionDate == date).ToList();
            response.ResonseCode = "00";
            response.ResponseMessage = "Transaction created successfully!";
            response.Data = transaction;

            return response;
        }

        public Response MakeDeposite(string AccountNumber, decimal Amount, string TransactionPin)
        {
            Response response = new Response();
            Account sourceAccount;
            Account destinationAccount;
            Transaction transaction = new Transaction();

            var authUser = _accountService.Authentication(AccountNumber, TransactionPin);
            if (authUser == null) throw new ApplicationException("Invalid credentials");
            try
            {
                //for deposit, our banksettlementaccout is the source of money the user a/c 
                sourceAccount = _accountService.GetByAccoutNumber(_ourBankSettlementAccount);
                destinationAccount = _accountService.GetByAccoutNumber(AccountNumber);

                sourceAccount.CurrentAccountBalance -= Amount;
                destinationAccount.CurrentAccountBalance += Amount;

                //check if there is update
                if ((_context.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) &&
                    (_context.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                {
                    transaction.TransactionStatus = Transaction.TranStatus.Success;
                    response.ResonseCode = "00";
                    response.ResponseMessage = "Transaction Successful!";
                }
                else
                {
                    transaction.TransactionStatus = Transaction.TranStatus.Failed;
                    response.ResonseCode = "01";
                    response.ResponseMessage = "Transaction failed!";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"An error occured ... => {ex.Message}");
            }

            //set other prop of transaction 
            transaction.TransactionType = Transaction.TranType.Deposite;
            transaction.TransactionSourceAccount = _ourBankSettlementAccount;
            transaction.TransactionDestinationAccount = AccountNumber;
            transaction.TransactionAmount = Amount;
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionParticulars = $"New Transaction from Source => {JsonConvert.SerializeObject(transaction.TransactionSourceAccount)} To Destination Accont => " +
                $"{JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)} on Date => {transaction.TransactionDate}" +
                $" for Amt =>{JsonConvert.SerializeObject(transaction.TransactionAmount)} Transaction Type => {JsonConvert.SerializeObject(transaction.TransactionType)}" +
                $"Transaction Status=>{JsonConvert.SerializeObject(transaction.TransactionStatus)}";

            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            return response;


        }

        public Response MakeFundsTransfer(string FromAccount, string ToAccount, decimal Amount, string TransactionPin)
        {
            Response response = new Response();
            Account sourceAccount;
            Account destinationAccount;
            Transaction transaction = new Transaction();

            var authUser = _accountService.Authentication(FromAccount, TransactionPin);
            if (authUser == null) throw new ApplicationException("Invalid credentials");
            try
            {
                //for withdtawl, our banksettlementaccout is the destination getting money from the user a/c 
                sourceAccount = _accountService.GetByAccoutNumber(FromAccount);
                destinationAccount = _accountService.GetByAccoutNumber(ToAccount);

                sourceAccount.CurrentAccountBalance -= Amount;
                destinationAccount.CurrentAccountBalance += Amount;

                //check if there is update
                if ((_context.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) &&
                    (_context.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                {
                    transaction.TransactionStatus = Transaction.TranStatus.Success;
                    response.ResonseCode = "00";
                    response.ResponseMessage = "Transaction Successfull!";
                }
                else
                {
                    transaction.TransactionStatus = Transaction.TranStatus.Failed;
                    response.ResonseCode = "01";
                    response.ResponseMessage = "Transaction failed!";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"An error occured ... => {ex.Message}");
            }

            //set other prop of transaction 
            transaction.TransactionType = Transaction.TranType.Transfer;
            transaction.TransactionSourceAccount = FromAccount;
            transaction.TransactionDestinationAccount = ToAccount;
            transaction.TransactionAmount = Amount;
            transaction.TransactionDate = DateTime.Now;

            string transactionDetails = $"New Transaction from Source => {JsonConvert.SerializeObject(transaction.TransactionSourceAccount)} " +
                $"To Destination Account => {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)} " +
                $"on Date => {transaction.TransactionDate} " +
                $"for Amount => {JsonConvert.SerializeObject(transaction.TransactionAmount)} " +
                $"Transaction Type => {JsonConvert.SerializeObject(transaction.TransactionType)} " +
                $"Transaction Status => {JsonConvert.SerializeObject(transaction.TransactionStatus)}";

            transaction.TransactionParticulars = transactionDetails;

            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            return response;
        }

        public Response MakeWithdrawal(string AccountNumber, decimal Amount, string TransactionPin)
        {
            //make withdtawl
            Response response = new Response();
            Account sourceAccount;
            Account destinationAccount;
            Transaction transaction = new Transaction();

            var authUser = _accountService.Authentication(AccountNumber, TransactionPin);
            if (authUser == null) throw new ApplicationException("Invalid credentials");
            try
            {
                //for withdtawl, our banksettlementaccout is the destination getting money from the user a/c 
                sourceAccount = _accountService.GetByAccoutNumber(AccountNumber);
                destinationAccount = _accountService.GetByAccoutNumber(_ourBankSettlementAccount);

                sourceAccount.CurrentAccountBalance -= Amount;
                destinationAccount.CurrentAccountBalance += Amount;

                //check if there is update
                if ((_context.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) &&
                    (_context.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                {
                    transaction.TransactionStatus = Transaction.TranStatus.Success;
                    response.ResonseCode = "00";
                    response.ResponseMessage = "Transaction Successful!";
                }
                else
                {
                    transaction.TransactionStatus = Transaction.TranStatus.Failed;
                    response.ResonseCode = "01";
                    response.ResponseMessage = "Transaction failed!";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"An error occured ... => {ex.Message}");
            }

            //set other prop of transaction 
            transaction.TransactionType = Transaction.TranType.Withdrawl;
            transaction.TransactionSourceAccount = AccountNumber;
            transaction.TransactionDestinationAccount = _ourBankSettlementAccount;
            transaction.TransactionAmount = Amount;
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionParticulars = $"New Transaction from Source => {JsonConvert.SerializeObject(transaction.TransactionSourceAccount)} To Destination Account => " +
                $"{JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)} on Date => {transaction.TransactionDate}" +
                $" for Amt =>{JsonConvert.SerializeObject(transaction.TransactionAmount)} Transaction Type => {JsonConvert.SerializeObject(transaction.TransactionType)}" +
                $"Transaction Status=>{JsonConvert.SerializeObject(transaction.TransactionStatus)}";

            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            return response;
        }
    }
}
