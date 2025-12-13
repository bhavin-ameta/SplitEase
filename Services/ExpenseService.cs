using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using SplitEase.DTO;
using SplitEase.Messages;
using SplitEase.Model;
using SplitEase.Models;
//using SplitEase.Models;
namespace SplitEase.Services
{
    public class ExpenseService : IExpensiveService
    {
        private readonly ApplicationDbContext _expensive;
        private readonly IJwtService _jwtService;
        private IMapper _mapper;


        public ExpenseService(ApplicationDbContext expensive, IJwtService jwtService, IMapper mapper)
        {
            _expensive = expensive;
            _jwtService = jwtService;
            _mapper = mapper;

        }

        public async Task<ApiResponse<ExpenseResponseDto>> AddExpenseAsync(AddExpenseDto dto)
        {
            if (!dto.IsPersonal)
            {
                var group = await _expensive.Groups
                             .FirstOrDefaultAsync(g => g.Id == dto.GroupId);
                if (group == null)
                    return ApiResponse<ExpenseResponseDto>.Fail(Message.GroupNotFound);

                bool isMember = await _expensive.GroupMembers
                    .AnyAsync(m => m.GroupId == dto.GroupId && m.UserId == dto.PaidByUserId);

                if (!isMember)
                    return ApiResponse<ExpenseResponseDto>.Fail(Message.NotMemberInGroup);
            }


            var user = await _expensive.UsersRegister.FirstOrDefaultAsync(g => g.UserId == dto.PaidByUserId);
            if (user == null)
            {
                return ApiResponse<ExpenseResponseDto>.Fail(Message.UserNotFound);
            }
            var newExpense = _mapper.Map<ExpenseModel>(dto);


            _expensive.Expenses.Add(newExpense);
            await _expensive.SaveChangesAsync();

            await _expensive.Entry(newExpense).Reference(e => e.Group).LoadAsync();
            await _expensive.Entry(newExpense).Reference(e => e.PaidByUser).LoadAsync();

            var response = _mapper.Map<ExpenseResponseDto>(newExpense);

            return ApiResponse<ExpenseResponseDto>.Success(Message.AddExpanse, response);



        }
        public async Task<ApiResponse<List<GetExpenseDto>>> GetExpenseAsync()
        {
            var expense = await _expensive.Expenses
                .Include(e => e.Group)
                .Include(e => e.PaidByUser).ToListAsync();

            if (expense == null)
            {
                return ApiResponse<List<GetExpenseDto>>.Fail(Message.UserNotFound);
            }

            var expensedto = _mapper.Map<List<GetExpenseDto>>(expense);


            return ApiResponse<List<GetExpenseDto>>.Success(Message.GetExpense, expensedto);

        }

        public async Task<ApiResponse<GetExpenseDto>> GetExpenseByIdAsync(Guid guid)
        {
            var expense = await _expensive.Expenses
            .Include(e => e.Group)
            .Include(e => e.PaidByUser)
            .FirstOrDefaultAsync(e => e.ExpenseId == guid);

            if (expense == null)
            {
                return ApiResponse<GetExpenseDto>.Fail(Message.UserNotFound);
            }

            var expensedto = _mapper.Map<GetExpenseDto>(expense);
            return ApiResponse<GetExpenseDto>.Success(Message.GetExpense, expensedto);
        }

        public async Task<ApiResponse<GetExpenseDto>> UpdateExpenseAsync(Guid id, UpdateExpenseDto dto)
        {
            var updateExpense = await _expensive.Expenses
                .Include(e => e.Group)
                .Include(e => e.PaidByUser).FirstOrDefaultAsync(e => e.ExpenseId == id);

            if (updateExpense == null)
            {
                return ApiResponse<GetExpenseDto>.Fail(Message.UserNotFound);
            }


            updateExpense.Description = dto.Description;
            updateExpense.Amount = dto.Amount;

            _expensive.Expenses.Update(updateExpense);
            await _expensive.SaveChangesAsync();

            var Expense = _mapper.Map<GetExpenseDto>(updateExpense);


            return ApiResponse<GetExpenseDto>.Success(Message.UpdateExpense, Expense);


        }

        public async Task<ApiResponse<bool>> DeleteExpenseASync(Guid guid)
        {
            var deleteexpense = await _expensive.Expenses.FindAsync(guid);
            if (deleteexpense == null)
            {
                return ApiResponse<bool>.Fail(Message.UserNotFound);
            }
            _expensive.Expenses.Remove(deleteexpense);
            await _expensive.SaveChangesAsync();


            return ApiResponse<bool>.Success(Message.DeleteExpense, true);

        }



    }

}
