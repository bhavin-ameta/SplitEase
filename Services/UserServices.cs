using AutoMapper;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using SplitEase.DTO;
using SplitEase.Messages;
using SplitEase.Model;
using SplitEase.Models;


namespace SplitEase.Services
{
    public class UserServices : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtService _jwtService;
        private IMapper _mapper;


        public UserServices(ApplicationDbContext context, IJwtService jwtService, IMapper mapper)
        {
            _context = context;
            _jwtService = jwtService;
            _mapper = mapper;
        }


        public async Task<ApiResponse<UserResponseDto>> RegisterUser(UserDto dto)
        {
            var existingUser = await _context.UsersRegister.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (existingUser != null)
            {
                return ApiResponse<UserResponseDto>.Fail(Message.EmailAlreadyExists);
            }
            var newUser = _mapper.Map<Usermodel>(dto);

            newUser.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            await _context.UsersRegister.AddAsync(newUser).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            var userResponse = _mapper.Map<UserResponseDto>(newUser);

            return ApiResponse<UserResponseDto>.Success(Message.RegistrationSuccess, userResponse);
        }

        public async Task<ApiResponse<LoginResponseDto>> LoginUser(LoginDto dto)
        {
            var email = dto.Email?.Trim().ToLower();

            var user = await _context.UsersRegister
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
            {
                return ApiResponse<LoginResponseDto>.Fail(Message.EmailNotFound);
            }
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);
            if (!isPasswordValid)
            {
                return ApiResponse<LoginResponseDto>.Fail(Message.InvalidPassword);
            }

            var token = _jwtService.GenerateToken(user);

            var userResponse = _mapper.Map<UserResponseDto>(user);

            var loginResponse = new LoginResponseDto
            {
                Token = token,
                User = userResponse
            };


            return ApiResponse<LoginResponseDto>.Success(Message.LoginSuccess, loginResponse);
        }

        public async Task<ApiResponse<List<Usermodel>>> GetUser()
        {
            var Users = await _context.UsersRegister.ToListAsync();
            if (Users == null)
            {
                return ApiResponse<List<Usermodel>>.Fail(Message.UserNotFound);
            }

            var mappedUsers = _mapper.Map<List<UserResponseDto>>(Users);
            return ApiResponse<List<Usermodel>>.Success(Message.UsersRetrieved, Users);
        }


        public async Task<ApiResponse<Usermodel>> GetUserById(Guid id)
        {
            var User = await _context.UsersRegister.FindAsync(id);
            if (User == null)
            {
                return ApiResponse<Usermodel>.Fail(Message.UserNotFound);
            }

            var userResponse = _mapper.Map<UserResponseDto>(User);
            return ApiResponse<Usermodel>.Success(Message.UsersRetrieved, User);
        }



        public async Task<ApiResponse<string>> ForgetPass(ForgetDto dto)
        {
            var user = await _context.UsersRegister.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
            {
                return ApiResponse<string>.Fail(Message.EmailNotFound);
            }

            var bytes = RandomNumberGenerator.GetBytes(64);
            string token = WebEncoders.Base64UrlEncode(bytes);

            user.ResetPasswordToken = token;
            user.ResetPasswordTokenExpiry = DateTime.Now.AddHours(1);

            await _context.SaveChangesAsync();

            string resetLink = $"https://Splitfrontend.com/reset-password?token={token}";

            return ApiResponse<string>.Success(Message.Pass_Link_Gen, resetLink);
        }

        public async Task<ApiResponse<bool>> ResetPass(NewPassword dto)
        {
            var User = await _context.UsersRegister.FirstOrDefaultAsync(u => u.ResetPasswordToken == dto.Token &&
            u.ResetPasswordTokenExpiry > DateTime.Now);

            if (User == null)
            {
                return ApiResponse<bool>.Fail(Message.InvalidToken);
            }

            User.Password = BCrypt.Net.BCrypt.HashPassword(dto.newPassword);

            User.ResetPasswordToken = null;
            User.ResetPasswordTokenExpiry = null;

            await _context.SaveChangesAsync();
            return ApiResponse<bool>.Success(Message.PasswordResetSuccess, true);
        }



        public async Task<ApiResponse<Usermodel>> UpdateUserProfile(UpdateProfileDto dto)
        {
            var User = await _context.UsersRegister.FindAsync(dto.UserId);

            if (User == null)
            {
                return ApiResponse<Usermodel>.Fail(Message.UserNotFound);
            }
            if (!string.IsNullOrEmpty(dto.FullName))
            {

                User.FullName = dto.FullName;
            }


            if (!string.IsNullOrEmpty(dto.Email))
            {
                User.Email = dto.Email;
            }

            var userResponse = _mapper.Map<UserResponseDto>(User);

            _context.UsersRegister.Update(User);
            await _context.SaveChangesAsync();

            return ApiResponse<Usermodel>.Success(Message.ProfileUpdated, User);


        }

        public async Task<ApiResponse<Usermodel>> UpdatePassword(UpdatePasswordDto dto)
        {
            var user = await _context.UsersRegister.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
                return ApiResponse<Usermodel>.Fail(Message.UserNotFound);

            bool isOldPasswordValid = BCrypt.Net.BCrypt.Verify(dto.OldPassword, user.Password);

            if (!isOldPasswordValid)
                return ApiResponse<Usermodel>.Fail(Message.OldPassword);

            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            var userResponse = _mapper.Map<UserResponseDto>(user);


            await _context.SaveChangesAsync();

            return ApiResponse<Usermodel>.Success(Message.PasswordChanged, user);
        }


        public async Task<ApiResponse<bool>> DeleteUser(Guid id)
        {
            var User = await _context.UsersRegister.FindAsync(id);

            if (User == null)
            {
                return ApiResponse<bool>.Fail(Message.UserNotFound);
            }
            _context.UsersRegister.Remove(User);
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.Success(Message.UserDeleted, true);
        }

    }

}
