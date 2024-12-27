using Microsoft.AspNetCore.Mvc;

namespace BlazorPOS.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MfaController : ControllerBase
    {
        private readonly IMfaService _mfaService;
        private readonly UserManager<ApplicationUser> _userManager;

        public MfaController(IMfaService mfaService, UserManager<ApplicationUser> userManager)
        {
            _mfaService = mfaService;
            _userManager = userManager;
        }

        [HttpPost("setup")]
        public async Task<IActionResult> SetupMfa([FromBody] MfaSetupDto model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
                return NotFound();

            user.MfaSecretKey = model.SecretKey;
            user.MfaEnabled = model.IsEnabled;

            await _userManager.UpdateAsync(user);
            return Ok();
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyCode([FromBody] MfaVerificationDto model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null || !user.MfaEnabled)
                return Unauthorized();

            var isValid = _mfaService.ValidateCode(user.MfaSecretKey, model.Code);
            return isValid ? Ok() : Unauthorized();
        }
    }
}