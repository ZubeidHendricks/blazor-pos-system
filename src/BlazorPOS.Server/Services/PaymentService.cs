using Stripe;

namespace BlazorPOS.Server.Services
{
    public class PaymentService : IPaymentService
    {
        public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
        {
            try 
            {
                var paymentIntent = await StripeClient.PaymentIntents.CreateAsync(new PaymentIntentCreateOptions
                {
                    Amount = (long)(request.Amount * 100),
                    Currency = "usd",
                    PaymentMethod = request.PaymentMethodId,
                    Confirm = true
                });

                return new PaymentResult 
                { 
                    Success = paymentIntent.Status == "succeeded",
                    TransactionId = paymentIntent.Id
                };
            }
            catch (StripeException ex)
            {
                return new PaymentResult 
                { 
                    Success = false, 
                    ErrorMessage = ex.Message 
                };
            }
        }
    }
}