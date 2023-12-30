using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using System.Text.RegularExpressions;
using TestBankAPI.Models;
using TestBankAPI.Services.Interface;

namespace TestBankAPI.Controllers
{
    [Route("api/v3/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IAccountService _accountService;
        IMapper _mapper;

        public AccountController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }
        [HttpPost]
        [Route("register_new_account")]
        public IActionResult RegisterNewAccount([FromBody] RegisterNewAccountModel newAccount)
        {
            if (ModelState.IsValid)
            {
                var account = _mapper.Map<Account>(newAccount);
                return Ok(_accountService.Create(account, newAccount.Pin, newAccount.ConfirmPin));
            }
            return BadRequest(newAccount);
        }

        [HttpGet]
        [Route("get_all_accounts")]
        public IActionResult GetAllAccount()
        {
            var accounts = _accountService.GetAllAccounts();
            var cleanedAccounts = _mapper.Map<IList<GetAccountModel>>(accounts);
            return Ok(cleanedAccounts);
        }
        [HttpPost]
        [Route("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticationModel model)
        {
            if (!ModelState.IsValid) return BadRequest(model);

            return Ok(_accountService.Authentication(model.AccountNumber, model.Pin));
        }

        [HttpGet]
        [Route("get_by_account_number")]
        public IActionResult GetByAccountNumber(string AccoutNumber)
        {
            if (!Regex.IsMatch(AccoutNumber, @"^[0-9]{10}$")) return BadRequest("Account number must be of 10-digit");
            var account = _accountService.GetByAccountNumber(AccoutNumber);
            var cleanedAccount = _mapper.Map<GetAccountModel>(account);
            return Ok(cleanedAccount);
        }


        [HttpGet]
        [Route("get_account_by_id")]
        public IActionResult GetByAccountById(int Id)
        {
            var account = _accountService.GetById(Id);
            var cleanedAccount = _mapper.Map<GetAccountModel>(account);
            return Ok(cleanedAccount);
        }

        [HttpPut]
        [Route("update_account")]
        public IActionResult UpdateAccount([FromBody] UpdateAccountModel model)
        {
            if (!ModelState.IsValid) return BadRequest(model);
            var account = _mapper.Map<Account>(model);
            _accountService.Update(account, model.Pin);
            return Ok();
        }
    }
}
