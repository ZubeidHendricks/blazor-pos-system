namespace BlazorPOS.Client.Models
{
    public class CashPayment
    {
        public decimal Amount { get; set; }
        public decimal TenderedAmount { get; set; }
        public decimal Change { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}