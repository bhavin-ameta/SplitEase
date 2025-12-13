using System.ComponentModel.DataAnnotations;

namespace SplitEase.DTO
{
    public class UserDto
    {
        [Required]
        public string? FullName { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
