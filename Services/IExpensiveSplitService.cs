using SplitEase.DTO;

namespace SplitEase.Services
{
    public interface IExpensiveSplitService
    {
        Task<ApiResponse<List<ExpenseSplitResponse>>> AddExpenseSplitAsync(AddExpenseSplitDto dto);
         Task<ApiResponse<List<SettlementResponse>>> CalculateSettlementAsync(Guid groupId);
        Task<ApiResponse<bool>> DeleteSplitAsync(Guid expenseSplitId);
       
    }
}
