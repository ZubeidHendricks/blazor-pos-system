using BlazorPOS.Client.Models;

namespace BlazorPOS.Client.Services
{
    public class PaymentService
    {
        private readonly HttpClient _httpClient;

        public PaymentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> ProcessCashPayment(decimal amount, decimal tendered)
        {
            var payment = new CashPayment
            {
                Amount = amount,
                TenderedAmount = tendered,
                Change = tendered - amount,
                PaymentDate = DateTime.Now
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/payments/cash", payment);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ProcessCardPayment(decimal amount)
        {
            var payment = new CardPayment
            {
                Amount = amount,
                PaymentDate = DateTime.Now,
                Status = "Pending"
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/payments/card", payment);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}