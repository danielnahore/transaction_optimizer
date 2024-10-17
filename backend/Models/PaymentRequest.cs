namespace AuthorizeNetApp.Models
{
    public class PaymentRequest
    {
        public string ApiLoginId { get; set; }
        public string TransactionKey { get; set; }
        public decimal Amount { get; set; }
        public string CardNumber { get; set; }
        public string ExpirationDate { get; set; }
        public string CardCode { get; set; }
    }
}