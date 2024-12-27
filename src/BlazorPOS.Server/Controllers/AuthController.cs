using Microsoft.AspNetCore.Mvc;
using BlazorPOS.Shared.Dtos;
using BlazorPOS.Shared.Models;

namespace BlazorPOS.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Unauthorized(new AuthResponseDto
                {
                    IsSuccess = false,
                    ErrorMessage = "Invalid login attempt."
                });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: true);
            if (result.Succeeded)
            {
                var token = _tokenService.GenerateJwtToken(user);
                return Ok(new AuthResponseDto
                {
                    IsSuccess = true,
                    Token = token
                });
            }

            return Unauthorized(new AuthResponseDto
            {
                IsSuccess = false,
                ErrorMessage = "Invalid login attempt."
            });
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto model)
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
            if (result.Succeeded)
            {
                var token = _tokenService.GenerateJwtToken(user);
                return Ok(new AuthResponseDto
                {
                    IsSuccess = true,
                    Token = token
                });
            }

            return BadRequest(new AuthResponseDto
            {
                IsSuccess = false,
                ErrorMessage = string.Join(", ", result.Errors.Select(e => e.Description))
            });
        }
    }
}