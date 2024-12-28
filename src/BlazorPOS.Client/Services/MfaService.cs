using System.Net.Http.Json;
using BlazorPOS.Shared.Dtos;

namespace BlazorPOS.Client.Services
{
    public interface IMfaClientService
    {
        Task<string> GenerateMfaQrCodeAsync(string userId);
        Task<bool> VerifyMfaCodeAsync(MfaVerificationDto model);
    }

    public class MfaClientService : IMfaClientService
    {
        private readonly HttpClient _httpClient;

        public MfaClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GenerateMfaQrCodeAsync(string userId)
        {
            var response = await _httpClient.GetStringAsync($"api/mfa/qrcode/{userId}");
            return response;
        }

        public async Task<bool> VerifyMfaCodeAsync(MfaVerificationDto model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/mfa/verify", model);
            return response.IsSuccessStatusCode;
        }
    }
}