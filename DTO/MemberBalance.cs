namespace SplitEase.DTO
{
    public class MemberBalance
    {
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public MemberBalance(Guid id, decimal amt) { UserId = id; Amount = amt; }
    }
}
