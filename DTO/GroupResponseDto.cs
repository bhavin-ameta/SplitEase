namespace SplitEase.DTO
{
    public class GroupResponseDto
    {
        public Guid Id { get; set; }
        public string? GroupName { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedByUserName { get; set; }

    }
}
