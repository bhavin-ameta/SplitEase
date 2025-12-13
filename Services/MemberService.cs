using AutoMapper;
using AutoMapper.Execution;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using SplitEase.DTO;
using SplitEase.Messages;
using SplitEase.Model;
using SplitEase.Models;

namespace SplitEase.Services
{
    public class MemberService : IMemberService
    {
        private readonly ApplicationDbContext _memberService;
        private readonly IJwtService _jwtService;
        private IMapper _mapper;

        public MemberService(ApplicationDbContext memberService, IJwtService jwtService, IMapper mapper)
        {
            _memberService = memberService;
            _jwtService = jwtService;
            _mapper = mapper;
        }

        public async Task<ApiResponse<GroupMemberDto>> AddMemberAsync(AddGroupMemberDto dto)
        {
            var Group = await _memberService.Groups.FirstOrDefaultAsync(g => g.Id == dto.GroupId);
            if (Group == null)
            {
                return ApiResponse<GroupMemberDto>.Fail(Message.GroupNotFound);
            }

            var user = await _memberService.UsersRegister.FirstOrDefaultAsync(u => u.UserId == dto.UserId);
            if (user == null)
            {
                return ApiResponse<GroupMemberDto>.Fail(Message.UserNotFound);
            }

            var exists = await _memberService.GroupMembers
                .FirstOrDefaultAsync(m => m.GroupId == dto.GroupId && m.UserId == dto.UserId);

            if (exists != null)
            {
                return ApiResponse<GroupMemberDto>.Fail(Message.AlreadyMember);
            }


            var newMember = _mapper.Map<GroupMember>(dto);
            newMember.JoinDate = DateTime.Now;


            _memberService.GroupMembers.Add(newMember);
            await _memberService.SaveChangesAsync();
            var memberDto = new GroupMemberDto
            {
                MemberId = newMember.MemberId,
                UserId = user.UserId,
                UserName = user.FullName,
                GroupName = Group.Name,
                IsAdmin = newMember.IsAdmin,
                JoinDate = newMember.JoinDate

            };

            return ApiResponse<GroupMemberDto>.Success(Message.MemberAdd, memberDto);

        }
        public async Task<ApiResponse<List<GetMemberDto>>> GetMemberAsync()
        {
            var GroupMember = await _memberService.GroupMembers
                  .Include(m => m.Group)
                  .Include(m => m.User)
                  .ToListAsync();

            if (GroupMember == null)
            {
                return ApiResponse<List<GetMemberDto>>.Fail(Message.UserNotFound);
            }

            var memberDto = _mapper.Map<List<GetMemberDto>>(GroupMember);
            return ApiResponse<List<GetMemberDto>>.Success(Message.GetMemberAdd, memberDto);

        }

        public async Task<ApiResponse<GetMemberDto>> GetMemberByIdAsync(Guid memberid)
        {
            var member = await _memberService.GroupMembers
                .Include(m => m.Group)
                .Include(m => m.User)
                .FirstOrDefaultAsync();

            if (member == null)
            {
                return ApiResponse<GetMemberDto>.Fail(Message.UserNotFound);
            }

            var memberDto = _mapper.Map<GetMemberDto>(member);

            return ApiResponse<GetMemberDto>.Success(Message.GetMemberAdd, memberDto);

        }

        public async Task<ApiResponse<GetMemberDto>> UpdateGroupMemberAsync(Guid id, MemberUpdatedDto dto)
        {
            var updateMember = await _memberService.GroupMembers
                                    .Include(m => m.Group)
                                    .Include(m => m.User).FirstOrDefaultAsync(m => m.MemberId == id);


            if (updateMember == null)
            {
                return ApiResponse<GetMemberDto>.Fail(Message.NotMemberFound);
            }

            if (updateMember.User != null)
            {
                updateMember.User.FullName = dto.UserName;
            }

            if (updateMember.Group != null)
            {
                updateMember.Group.Name = dto.GroupName;
            }

            updateMember.IsAdmin = dto.IsAdmin;

            //_memberService.UpdateMember(updateMember);
            await _memberService.SaveChangesAsync();

            var member = _mapper.Map<GetMemberDto>(updateMember);

            return ApiResponse<GetMemberDto>.Success(Message.UpdatedMember, member);

        }

        public async Task<ApiResponse<bool>> DeleteMemberAsync(Guid id)
        {
            var DeleteMember = await _memberService.GroupMembers.FindAsync(id);
            if (DeleteMember == null)
            {
                return ApiResponse<bool>.Fail(Message.NotMemberFound);
            }

            _memberService.GroupMembers.Remove(DeleteMember);
            await _memberService.SaveChangesAsync();

            return ApiResponse<bool>.Success(Message.DeleteMember);
        }
    }
}
