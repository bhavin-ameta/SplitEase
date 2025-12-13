using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using SplitEase.DTO;

namespace SplitEase.Model
{
    public class Usermodel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserId {  get; set; }
        [Required(ErrorMessage = "Full name is required.")]
        public string? FullName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [JsonIgnore]
        public string? Password {  get; set; }

        public string? ResetPasswordToken { get; set; }

        public DateTime? ResetPasswordTokenExpiry { get; set; }


    }
}
