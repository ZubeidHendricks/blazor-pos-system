using System.Net.Http.Json;
using BlazorPOS.Shared.Dtos;

namespace BlazorPOS.Client.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authStateProvider;

        public AuthenticationService(
            HttpClient httpClient, 
            AuthenticationStateProvider authStateProvider)
        {
            _httpClient = httpClient;
            _authStateProvider = authStateProvider;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginModel)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginModel);
            return await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerModel)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", registerModel);
            return await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        }
    }

    public interface IAuthenticationService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto loginModel);
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerModel);
    }
}