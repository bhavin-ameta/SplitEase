namespace SplitEase.DTO
{
    public class SettlementResponse
    {
        public Guid FromUserId { get; set; }
        public string? FromUserName { get; set; }
        public Guid ToUserId { get; set; }
        public string? ToUserName { get; set; }

        public decimal Amount { get; set; }

        public string? Summary { get; set; }
    }
}
