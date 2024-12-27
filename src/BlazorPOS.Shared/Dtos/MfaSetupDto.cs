namespace BlazorPOS.Shared.Dtos
{
    public class MfaSetupDto
    {
        public string UserId { get; set; }
        public string SecretKey { get; set; }
        public bool IsEnabled { get; set; }
    }

    public class MfaVerificationDto
    {
        public string UserId { get; set; }
        public string Code { get; set; }
    }
}