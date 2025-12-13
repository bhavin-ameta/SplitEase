namespace SplitEase.DTO
{
    public class GetMemberDto
    {
        public Guid MemberId { get; set; }
        public string? GroupName { get; set; }
        public string? UserName { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime JoinDate { get; set; }
    }
}

