using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace SplitEase.Model
{
    public class GroupMember
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid MemberId { get; set; }

        public Guid GroupId { get; set; }
        public Group_Model? Group {get; set;}
        public Guid UserId { get; set;}

        public Usermodel? User { get; set;}

        [Required]
        [DataType(DataType.Date)]
        public DateTime JoinDate { get; set; }= DateTime.Now;
        public bool IsAdmin { get; set; }   

    }
}
