using SplitEase.DTO;
using SplitEase.Model;

namespace SplitEase.Services
{
    public interface IGroupService
    {
        Task<ApiResponse<GroupResponseDto>> AddGroupAsync(GroupDto dto);
        Task<ApiResponse<List<GetGroupDto>>> GetGroupAsync();
        Task<ApiResponse<GetGroupDto>> GetGroupByIdAsync(Guid id);
        Task<ApiResponse<GroupResponseDto>> UpdateGroupAsync(Guid id, UpdateGroup dto);
        Task<ApiResponse<bool>> DeleteGroupAsync(Guid id);
    }
}
