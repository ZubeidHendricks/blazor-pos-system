using System.Security.Cryptography;
using OtpNet;

namespace BlazorPOS.Server.Services
{
    public interface IMfaService
    {
        string GenerateSecretKey();
        bool ValidateCode(string secretKey, string code);
        string GetQrCodeUrl(string email, string secretKey);
    }

    public class MfaService : IMfaService
    {
        public string GenerateSecretKey()
        {
            var key = new byte[20];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(key);
            }
            return Base32Encoding.ToString(key);
        }

        public bool ValidateCode(string secretKey, string code)
        {
            var totp = new Totp(Base32Encoding.ToBytes(secretKey));
            return totp.VerifyTotp(code, out _, VerificationWindow.RfcSpecifiedNetworkTimeStep);
        }

        public string GetQrCodeUrl(string email, string secretKey)
        {
            var issuer = "BlazorPOS";
            var url = $"otpauth://totp/{issuer}:{email}?secret={secretKey}&issuer={issuer}";
            return $"https://api.qrserver.com/v1/create-qr-code/?data={Uri.EscapeDataString(url)}";
        }
    }
}