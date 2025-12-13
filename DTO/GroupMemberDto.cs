namespace SplitEase.DTO
{
    public class GroupMemberDto
    {
        public Guid MemberId { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public string? GroupName {  get; set; } 
        public bool IsAdmin { get; set; }
        public DateTime JoinDate { get; set; }
    }
}
