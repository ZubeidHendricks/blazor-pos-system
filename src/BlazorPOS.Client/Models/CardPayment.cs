namespace BlazorPOS.Client.Models
{
    public class CardPayment
    {
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Status { get; set; }
        public string? TransactionId { get; set; }
    }
}