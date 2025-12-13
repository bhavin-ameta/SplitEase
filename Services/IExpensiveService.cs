using SplitEase.DTO;

namespace SplitEase.Services
{
    public interface IExpensiveService
    {
        Task<ApiResponse<ExpenseResponseDto>> AddExpenseAsync(AddExpenseDto dto);
        Task<ApiResponse<List<GetExpenseDto>>> GetExpenseAsync();
        Task<ApiResponse<GetExpenseDto>> GetExpenseByIdAsync(Guid guid);
        Task<ApiResponse<GetExpenseDto>> UpdateExpenseAsync(Guid id, UpdateExpenseDto dto);
        Task<ApiResponse<bool>> DeleteExpenseASync(Guid guid);

    }
}
