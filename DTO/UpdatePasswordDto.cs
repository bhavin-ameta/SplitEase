using System.ComponentModel.DataAnnotations;

namespace SplitEase.DTO
{
    public class UpdatePasswordDto
    {
        [EmailAddress]
        public string? Email { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
    }
}
