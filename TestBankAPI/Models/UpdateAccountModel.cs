using System.ComponentModel.DataAnnotations;

namespace TestBankAPI.Models
{
    public class UpdateAccountModel
    {
        [Key]
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]/d{4}$", ErrorMessage = "Pin must not be more than 4 digits")]
        public string Pin { get; set; }
        [Compare("Pin", ErrorMessage = "Pin do not match")]
        public string ConfirmPin { get; set; }
        public DateTime DateLastUpdated { get; set; }

    }
}

