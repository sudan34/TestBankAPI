namespace TestBankAPI.Models
{
    public class Response
    {
        public string RequestId => $"{Guid.NewGuid().ToString()}";
        public string ResonseCode { get; set; }
        public string ResponseMessage { get; set; }
        public object Date {  get; set; }
    }
}
