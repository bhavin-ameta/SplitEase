using System.ComponentModel.DataAnnotations;
using SplitEase.Model;

namespace SplitEase.DTO
{
    public class GroupDto
    {
        [Required]
        public string? GroupName { get; set; }

        public string? Description { get; set; }
        [Required]
        public Guid CreatedByUserId { get; set; }
        //public Usermodel? CreatedByUser { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }
    }
}
