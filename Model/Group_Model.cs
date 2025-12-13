using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SplitEase.Model
{
    public class Group_Model
    {
     [Key]
     [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
     public Guid Id { get; set; }
     [Required]
     public string? Name { get; set; }

     public string? Description { get; set; }
     [Required]
     public Guid CreatedByUserId { get; set; }
     public Usermodel? CreatedByUser { get; set; }

     [Required]
     [DataType(DataType.Date)]
     public DateTime CreatedDate { get; set; }


     public List<GroupMember> Members { get; set; } = new List<GroupMember>();
     public List<ExpenseModel>? Expenses { get; set; } = new List<ExpenseModel>();                         


    }
}
