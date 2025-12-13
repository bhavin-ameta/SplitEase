using SplitEase.DTO;
using SplitEase.Model;

namespace SplitEase.Services
{
    public interface IMemberService
    {

        Task<ApiResponse<GroupMemberDto>> AddMemberAsync(AddGroupMemberDto dto);
        Task<ApiResponse<List<GetMemberDto>>> GetMemberAsync();
        Task<ApiResponse<GetMemberDto>> GetMemberByIdAsync(Guid memberid);
        Task<ApiResponse<GetMemberDto>> UpdateGroupMemberAsync(Guid id, MemberUpdatedDto dto);
        Task<ApiResponse<bool>> DeleteMemberAsync(Guid id);
    }
}
