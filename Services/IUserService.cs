using SplitEase.DTO;
using SplitEase.Model;

namespace SplitEase.Services
{
 
   public interface IUserService
        {
            Task<ApiResponse<UserResponseDto>> RegisterUser(UserDto dto);
           Task<ApiResponse<LoginResponseDto>> LoginUser(LoginDto dto);
            Task<ApiResponse<List<Usermodel>>> GetUser();
            Task<ApiResponse<string>> ForgetPass(ForgetDto dto);
            Task<ApiResponse<bool>> ResetPass(NewPassword dto);
            Task<ApiResponse<Usermodel>> GetUserById(Guid id);
            Task<ApiResponse<Usermodel>> UpdateUserProfile(UpdateProfileDto dto);
            Task<ApiResponse<Usermodel>> UpdatePassword(UpdatePasswordDto dto);
            Task<ApiResponse<bool>> DeleteUser(Guid id);



    }
     

}
