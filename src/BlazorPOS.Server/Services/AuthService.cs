using Microsoft.AspNetCore.Identity;
using BlazorPOS.Shared.Models;

namespace BlazorPOS.Server.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthService(
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<AuthResult> RegisterAsync(RegisterModel model)
        {
            var user = new ApplicationUser 
            { 
                UserName = model.Email, 
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Role = model.Role 
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            return new AuthResult 
            { 
                Succeeded = result.Succeeded, 
                Errors = result.Errors.Select(e => e.Description).ToList() 
            };
        }

        public async Task<AuthResult> LoginAsync(LoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(
                model.Email, 
                model.Password, 
                model.RememberMe, 
                lockoutOnFailure: true
            );

            return new AuthResult { Succeeded = result.Succeeded };
        }
    }
}