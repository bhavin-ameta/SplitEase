using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SplitEase.Model
{
    public class ExpenseSplit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ExpenseSplitId { get; set; }
        [Required]
        public Guid ExpenseId { get; set; }
        
        public ExpenseModel? Expense { get; set; }

        [Required]
        public Guid UserId { get; set; }
        
        public Usermodel? User { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ShareAmount { get; set; }
    }
}
