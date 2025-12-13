using SplitEase.Model;

namespace SplitEase.DTO
{
    public class GetGroupDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedByUserName { get; set; }
        public List<GetMemberDto>? Members { get; set; }
    }
}
