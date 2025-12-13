using System.ComponentModel.DataAnnotations;

namespace SplitEase.DTO
{
    public class ForgetDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

    }
}
