using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TestBankAPI.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccountName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public decimal CurrentAccountBalance { get; set; }
        public AccountType AccountType { get; set; }
        public string AccountNumberGenerated {  get; set; }
        [JsonIgnore]
        public byte[] PinHash { get; set; }
        [JsonIgnore]
        public byte[] PinSalt { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastUpdated { get; set; }
        //Generate accoutnt number
        Random rand = new Random();

        public Account() 
        {
            AccountNumberGenerated = GenerateRandomAccountNumber();
            AccountName = $"{FirstName} {LastName}";
        }
        private string GenerateRandomAccountNumber()
        {
            Random rand = new Random();
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, 10)
              .Select(s => s[rand.Next(s.Length)]).ToArray());
        }
    }
    public enum AccountType { Saving, Current, Corporate, Government }
}
