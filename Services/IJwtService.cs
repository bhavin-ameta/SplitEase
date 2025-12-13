using SplitEase.Model;

namespace SplitEase.Services
{
    public interface IJwtService
    {
        string GenerateToken(Usermodel user);
        string GeneratePasswordResetToken(Usermodel user);
    }
}
