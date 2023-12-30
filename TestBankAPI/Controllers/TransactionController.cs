using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using TestBankAPI.Models;
using TestBankAPI.Services.Interface;

namespace TestBankAPI.Controllers
{
    [Route("api/v3/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private ITransactionService _transactionService;
        IMapper _mapper;

        public TransactionController(ITransactionService transactionService, IMapper mapper)
        {
            _transactionService = transactionService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("create_new_transaction")]
        public IActionResult CreateNewTransaction([FromBody] TransactionRequestDto transactionRequest)
        {
            if (!ModelState.IsValid) return BadRequest(transactionRequest);

            var transaction = _mapper.Map<Transaction>(transactionRequest);
            return Ok(_transactionService.CreateNewTransaction(transaction));
        }
        [HttpPost]
        [Route("make_deposite")]
        public IActionResult MakeDeposite(string AccountNumber, decimal Amount, string TransactionPin)
        {
            if (!Regex.IsMatch(AccountNumber, @"^[0-9]{10}$")) return BadRequest("Account number must be of 10-digit");
            return Ok(_transactionService.MakeDeposite(AccountNumber, Amount, TransactionPin));
        }
        [HttpPost]
        [Route("make_withdrawal")]
        public IActionResult MakeWithdrawal(string AccountNumber, decimal Amount, string TransactionPin)
        {
            if (!Regex.IsMatch(AccountNumber, @"[0][1-9]\d{9}$|^[1-9]/d{9}$")) return BadRequest("Account number must be of 10-digit");
            return Ok(_transactionService.MakeWithdrawal(AccountNumber, Amount, TransactionPin));
        }
        [HttpPost]
        [Route("make_funds_transfer")]
        public IActionResult MakeFundsTransfer(string FromAccount, string ToAccount, decimal Amount, string TransactionPin)
        {
            if (!Regex.IsMatch(FromAccount, @"[0][1-9]\d{9}$|^[1-9]/d{9}$") || !Regex.IsMatch(ToAccount, @"[0][1-9]\d{9}$|^[1-9]/d{9}$"))
                return BadRequest("Account number must be of 10-digit");

            return Ok(_transactionService.MakeFundsTransfer(FromAccount,ToAccount, Amount, TransactionPin));
        }
    }
}
