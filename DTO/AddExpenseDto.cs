using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SplitEase.Model;

namespace SplitEase.DTO
{
    public class AddExpenseDto
    {
        public Guid? GroupId { get; set; }
        public bool IsPersonal { get; set; }
        public Guid PaidByUserId { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public string? Description { get; set; }

    }
}
