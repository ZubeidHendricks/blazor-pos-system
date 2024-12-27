using Microsoft.AspNetCore.Identity;

namespace BlazorPOS.Shared.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public string MfaSecretKey { get; set; }
        public bool MfaEnabled { get; set; } = false;
    }
}