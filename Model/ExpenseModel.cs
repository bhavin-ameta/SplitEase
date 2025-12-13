using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace SplitEase.Model
{
    public class ExpenseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ExpenseId { get; set; }

        //[Required]
        public Guid? GroupId { get; set; }
        
        public Group_Model? Group { get; set; }

        
        public Guid PaidByUserId { get; set; }
        
        public Usermodel? PaidByUser { get; set; }
        //[Required]
        //[StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        //public string? Title {  get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [StringLength(250, ErrorMessage = "Description cannot exceed 250 characters")]
        public string? Description { get; set; }

        public bool IsPersonal { get; set; }


        public List<ExpenseSplit>? Splits { get; set; }



    }
}
