using System.ComponentModel.DataAnnotations;

namespace TestBankAPI.Models
{
    public class RegisterNewAccountModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public AccountType AccountType { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastUpdated { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]{4}$", ErrorMessage="Pin must not be more than 4 digits")]
        public string Pin { get;set; }
        [Compare("Pin", ErrorMessage = "Pin do not match")]
        public string ConfirmPin { get; set; }
    }
}
