using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SplitEase.DTO
{
    public class ExpenseSplitDto
    {

        //public Guid ExpenseId {  get; set; }    
        [Required]
        public Guid UserId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ShareAmount { get; set; }
        //public decimal Percentage { get; set; }
    }


}
