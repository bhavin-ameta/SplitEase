namespace SplitEase.DTO
{
    public class ResetPasswordDto
    {
        public string? ResetPasswordToken { get; set; }
        public DateTime? ResetPasswordTokenExpiry { get; set; }
    }
}
