using SplitEase.Model;

namespace SplitEase.DTO
{
    public class GetExpenseDto
    {
        public Guid ExpenseId { get; set; }
        public string? GroupName { get; set; }
        public string? Description { get; set; }
        public Guid PaidByUserId { get; set; }
        public string? PaidByUserName { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }



    }
}
