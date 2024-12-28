using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BlazorPOS.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly ISecurityAuditService _auditService;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService,
            ISecurityAuditService auditService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _auditService = auditService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                await _auditService.LogLoginAttempt(null, GetIpAddress(), false);
                return Unauthorized(new AuthResponseDto { ErrorMessage = "Invalid login attempt." });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: true);
            if (result.Succeeded)
            {
                var token = _tokenService.GenerateJwtToken(user);
                await _auditService.LogLoginAttempt(user.Id, GetIpAddress(), true);

                return Ok(new AuthResponseDto { Token = token, IsSuccess = true });
            }

            await _auditService.LogLoginAttempt(user.Id, GetIpAddress(), false);
            return Unauthorized(new AuthResponseDto { ErrorMessage = "Invalid login attempt." });
        }

        private string GetIpAddress()
        {
            return HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        }
    }
}