using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SplitEase.DTO;
using SplitEase.Messages;

//using SplitEase.Messages;
using SplitEase.Model;
using SplitEase.Models;

namespace SplitEase.Services
{
    public class GroupService : IGroupService
    {
        private readonly ApplicationDbContext _groupContex;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        public GroupService(ApplicationDbContext groupContex, IJwtService jwtService, IMapper mapper)
        {
            _groupContex = groupContex;
            _jwtService = jwtService;
            _mapper = mapper;
        }

        public async Task<ApiResponse<GroupResponseDto>> AddGroupAsync(GroupDto dto)
        {
            var existingGroup = await _groupContex.Groups
             .FirstOrDefaultAsync(g => g.Name == dto.GroupName
                           && g.CreatedByUserId == dto.CreatedByUserId);

            if (existingGroup != null)
            {
                return ApiResponse<GroupResponseDto>.Fail(Message.GroupAlreadyExists);
            }

            var newGroup = _mapper.Map<Group_Model>(dto);

            await _groupContex.Groups.AddAsync(newGroup);
            await _groupContex.SaveChangesAsync();

            var creatorMember = new GroupMember
            {
                GroupId = newGroup.Id,
                UserId = dto.CreatedByUserId,
                IsAdmin = true,
                JoinDate = DateTime.Now
            };

            await _groupContex.GroupMembers.AddAsync(creatorMember);
            await _groupContex.SaveChangesAsync();

            await _groupContex.Entry(newGroup).Reference(g => g.CreatedByUser).LoadAsync();
            var responseDto = _mapper.Map<GroupResponseDto>(newGroup);

            return ApiResponse<GroupResponseDto>.Success(Message.NewGroup, responseDto);
        }

        public async Task<ApiResponse<List<GetGroupDto>>> GetGroupAsync()
        {
            var groups = await _groupContex.Groups
            .Include(g => g.CreatedByUser)
            .Include(g => g.Members)
            .ThenInclude(g => g.User)
            .ToListAsync();
            var groupDtos = _mapper.Map<List<GetGroupDto>>(groups);
            if (groups == null)
            {
                return ApiResponse<List<GetGroupDto>>.Fail(Message.GroupNotFound);
            }

            return ApiResponse<List<GetGroupDto>>.Success(Message.GroupFound, groupDtos);
        }

        public async Task<ApiResponse<GetGroupDto>> GetGroupByIdAsync(Guid id)
        {
            var group = await _groupContex.Groups
                .Include(g => g.CreatedByUser)
                .Include(g => g.Members)
                .ThenInclude(m => m.User)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (group == null)
            {
                return ApiResponse<GetGroupDto>.Fail(Message.GroupNotFound);
            }

            var dto = _mapper.Map<GetGroupDto>(group);

            return ApiResponse<GetGroupDto>.Success(Message.GroupFound, dto);

        }

        public async Task<ApiResponse<GroupResponseDto>> UpdateGroupAsync(Guid id, UpdateGroup dto)
        {

            var UpdateGroup = await _groupContex.Groups
                .Include(g => g.CreatedByUser)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (UpdateGroup == null)
            {
                return ApiResponse<GroupResponseDto>.Fail(Message.GroupNotFound);
            }

            UpdateGroup.Name = dto.Name;
            UpdateGroup.Description = dto.Description;

            await _groupContex.SaveChangesAsync();
            await _groupContex.Entry(UpdateGroup).Reference(g => g.CreatedByUser).LoadAsync();

            var result = _mapper.Map<GroupResponseDto>(UpdateGroup);

            return ApiResponse<GroupResponseDto>.Success(Message.UpdateGroup, result);
        }


        public async Task<ApiResponse<bool>> DeleteGroupAsync(Guid id)
        {
            var DeleteGroup = await _groupContex.Groups.FindAsync(id);

            if (DeleteGroup == null)
            {
                return ApiResponse<bool>.Fail(Message.GroupNotFound);
            }
            _groupContex.Groups.Remove(DeleteGroup);
            await _groupContex.SaveChangesAsync();

            return ApiResponse<bool>.Success(Message.DeleteGroup);

        }


    }
}

