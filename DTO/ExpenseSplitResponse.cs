namespace SplitEase.DTO
{
    public class ExpenseSplitResponse
    {
        public Guid ExpenseSplitId { get; set; }
        public Guid ExpenseId { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public decimal ShareAmount { get; set; }
        public string? Summary { get; set; }

    }
}
