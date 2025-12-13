using System.ComponentModel.DataAnnotations;

namespace SplitEase.DTO
{


    public class AddExpenseSplitDto
    {
        [Required]
        public Guid ExpenseId { get; set; }

        public Guid GroupId {  get; set; }

        public string? SplitType { get; set; }


        [Required]
        public List<ExpenseSplitDto> ?Splits { get; set; }
    }

}

