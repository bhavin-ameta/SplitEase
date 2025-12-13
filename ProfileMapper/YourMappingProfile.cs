using AutoMapper;
using SplitEase.DTO;
using SplitEase.Model;

namespace SplitEase.ProfileMapper
{
    public class YourMappingProfile:Profile
    {
        public YourMappingProfile()
        {
            CreateMap<UserDto, Usermodel>()
            .ForMember(dest => dest.Password, opt => opt.Ignore());

            CreateMap<Usermodel, UserResponseDto>();

            CreateMap<UpdateProfileDto, Usermodel>();
            CreateMap<UpdatePasswordDto, Usermodel>()
            .ForMember(dest => dest.Password, opt => opt.Ignore());

            CreateMap<GroupDto, Group_Model>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.GroupName))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
            .ForMember(dest => dest.CreatedByUserId, opt => opt.MapFrom(src => src.CreatedByUserId));

            CreateMap<Group_Model, GroupResponseDto>()
            .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.CreatedByUserName,opt => opt.MapFrom(src => src.CreatedByUser != null ? src.CreatedByUser.FullName : null));

            CreateMap<GroupResponseDto, UpdateGroup>().ReverseMap();
              
            CreateMap<Group_Model, GetGroupDto>()
             .ForMember(dest => dest.CreatedByUserName,
               opt => opt.MapFrom(src => src.CreatedByUser != null? src.CreatedByUser.FullName: null));


            CreateMap<GroupMember, GroupMemberDto>()
             .ForMember(dest => dest.MemberId, opt => opt.MapFrom(src => src.MemberId))
             .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
             .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User!= null ? src.User.FullName:null));


            CreateMap<GroupMember, AddGroupMemberDto>().ReverseMap();

            CreateMap<GroupMember, GroupMemberDto>().ReverseMap();

            CreateMap<AddGroupMemberDto, GroupMemberDto>().ReverseMap();

            CreateMap<GroupMember, GetMemberDto>()
            .ForMember(dest => dest.GroupName,
              opt => opt.MapFrom(src => src.Group != null ? src.Group.Name : null))
            .ForMember(dest => dest.UserName,
              opt => opt.MapFrom(src => src.User != null ? src.User.FullName : null));


            CreateMap<ExpenseModel, ExpenseResponseDto>()
            .ForMember(dest => dest.GroupName,
              opt => opt.MapFrom(src => src.Group != null ? src.Group.Name : null))
            .ForMember(dest => dest.PaidByUserName,
              opt => opt.MapFrom(src => src.PaidByUser != null ? src.PaidByUser.FullName : null));
            CreateMap<AddExpenseDto, ExpenseModel>()
            .ForMember(dest => dest.GroupId,
              opt => opt.MapFrom(src => src.IsPersonal ? (Guid?)null : src.GroupId));

            CreateMap<ExpenseModel,GetExpenseDto>()
            .ForMember(dest => dest.GroupName,
              opt => opt.MapFrom(src => src.Group != null ? src.Group.Name : null))
            .ForMember(dest => dest.PaidByUserName,
              opt => opt.MapFrom(src => src.PaidByUser != null ? src.PaidByUser.FullName : null));

            CreateMap<ExpenseModel, GetExpenseDto>()
            .ForMember(dest => dest.GroupName,
              opt => opt.MapFrom(src => src.Group != null ? src.Group.Name : null))
            .ForMember(dest => dest.PaidByUserName,
              opt => opt.MapFrom(src => src.PaidByUser != null ? src.PaidByUser.FullName : null));


            CreateMap<ExpenseSplit, ExpenseSplitResponse>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src =>
             src.User != null ? src.User.FullName : string.Empty))
            .ForMember(dest => dest.Summary,
               opt => opt.Ignore());


            CreateMap<MemberBalance, SettlementResponse>()
            .ForMember(dest => dest.FromUserId, opt => opt.Ignore())
            .ForMember(dest => dest.FromUserName, opt => opt.Ignore())
            .ForMember(dest => dest.ToUserId, opt => opt.Ignore())
            .ForMember(dest => dest.ToUserName, opt => opt.Ignore())
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => Math.Round(src.Amount, 2)))
            .ForMember(dest => dest.Summary, opt => opt.Ignore());

        }
    }
}
