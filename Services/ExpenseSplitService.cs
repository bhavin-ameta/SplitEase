using AutoMapper;
using AutoMapper.Execution;
using Azure;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using SplitEase.DTO;
using SplitEase.Messages;
using SplitEase.Model;
using SplitEase.Models;

namespace SplitEase.Services
{
    public class ExpenseSplitService : IExpensiveSplitService
    {
        private readonly ApplicationDbContext _expenseSplit;
        //private readonly JwtService _jwtService;
        private IMapper _mapper;

        public ExpenseSplitService(ApplicationDbContext expenseSplit, IMapper mapper)
        {
            _expenseSplit = expenseSplit;
            //_jwtService = jwtService;
            _mapper = mapper;

        }
        public async Task<ApiResponse<List<ExpenseSplitResponse>>> AddExpenseSplitAsync(AddExpenseSplitDto dto)
        {
            var splitsList = dto.Splits ?? new List<ExpenseSplitDto>();

            var expense = await _expenseSplit.Expenses
                .FirstOrDefaultAsync(x => x.ExpenseId == dto.ExpenseId);

            if (expense == null)
            {
                return ApiResponse<List<ExpenseSplitResponse>>.Fail(Message.NotExpense);
            }

            var groupUsers = await _expenseSplit.GroupMembers
                .Where(x => x.GroupId == dto.GroupId)
                .Select(x => x.UserId)
                .ToListAsync();

            if (!groupUsers.Any())
            {
                return ApiResponse<List<ExpenseSplitResponse>>.Fail(Message.GroupNotFound);
            }

            var oldSplits = await _expenseSplit.ExpensesSplit
                .Where(x => x.ExpenseId == dto.ExpenseId)
                 .ToListAsync();

            if (oldSplits.Any())
            {
                _expenseSplit.ExpensesSplit.RemoveRange(oldSplits);
                await _expenseSplit.SaveChangesAsync();
            }

            List<ExpenseSplit> splits = new List<ExpenseSplit>();

            string summary = "";

            if (dto.SplitType?.ToLower() == "equal")
            {
                decimal equalShare = Math.Round(expense.Amount / groupUsers.Count, 2);

                splits = groupUsers.Select(id => new ExpenseSplit
                {
                    ExpenseId = dto.ExpenseId,
                    UserId = id,
                    ShareAmount = equalShare
                }).ToList();

                summary = $"Total ₹{expense.Amount} was split equally among {groupUsers.Count} members. " +
                   $"Each member will pay ₹{equalShare}.";

            }



            else if (dto.SplitType?.ToLower() == "custom")
            {
                decimal totalCustomShare = splitsList.Sum(x => x.ShareAmount);

                if (totalCustomShare != expense.Amount)
                {
                    return ApiResponse<List<ExpenseSplitResponse>>.Fail(Message.ExpenseEqual);
                }

                splits = splitsList.Select(s => new ExpenseSplit
                {
                    ExpenseId = dto.ExpenseId,
                    UserId = s.UserId,
                    ShareAmount = s.ShareAmount
                }).ToList();


                var userNames = await _expenseSplit.UsersRegister
                     .Where(u => splitsList.Select(x => x.UserId).Contains(u.UserId))
                     .ToDictionaryAsync(x => x.UserId, x => x.FullName);

                summary = $"Total ₹{expense.Amount} was split among {splitsList.Count} members. Shares: " +
                          string.Join(", ", splitsList.Select(s =>
                          $"{(userNames.ContainsKey(s.UserId) ? userNames[s.UserId] : s.UserId.ToString())} pays ₹{Math.Round(
                           splits.First(split => split.UserId == s.UserId).ShareAmount, 2)}")) + ".";
            }

            else
            {
                return ApiResponse<List<ExpenseSplitResponse>>.Fail(Message.InvalidSplit);
            }


            await _expenseSplit.ExpensesSplit.AddRangeAsync(splits);
            await _expenseSplit.SaveChangesAsync();


            var response = _mapper.Map<List<ExpenseSplitResponse>>(splits);

            var responseUserNames = await _expenseSplit.UsersRegister
            .Where(u => splits.Select(s => s.UserId).Contains(u.UserId))
            .ToDictionaryAsync(u => u.UserId, u => u.FullName);

            foreach (var item in response)
            {
                item.UserName = responseUserNames.GetValueOrDefault(item.UserId);
                item.Summary = summary; // ⭐ IMPORTANT
            }

            return ApiResponse<List<ExpenseSplitResponse>>.Success(Message.ExpenseSplitAdd, response);

        }

        public async Task<ApiResponse<List<SettlementResponse>>> CalculateSettlementAsync(Guid groupId)
        {
            var expenses = await _expenseSplit.Expenses
                .Where(e => e.GroupId == groupId)
                .ToListAsync();

            if (!expenses.Any())
            {
                return ApiResponse<List<SettlementResponse>>.Fail(Message.NotExpense);
            }


            var userIds = expenses.Select(e => e.PaidByUserId).Distinct().ToList();

            int memberCount = userIds.Count;


            decimal totalExpense = Math.Round(expenses.Sum(e => e.Amount), 2);


            decimal perUserShare = Math.Round(totalExpense / memberCount, 2);


            var paid = expenses
                .GroupBy(e => e.PaidByUserId)
                .ToDictionary(g => g.Key, g => g.Sum(e => e.Amount));


            var balance = userIds.ToDictionary(u => u,
                u => paid.GetValueOrDefault(u, 0) - perUserShare);

            var creditors = balance
                .Where(x => x.Value > 0)
                .Select(x => new MemberBalance(x.Key, x.Value))
                .OrderByDescending(c => c.Amount)
                .ToList();

            var debtors = balance
                .Where(x => x.Value < 0)
                .Select(x => new MemberBalance(x.Key, -x.Value))
                .OrderByDescending(d => d.Amount)
                .ToList();

            var userNames = await _expenseSplit.UsersRegister
                .Where(u => userIds.Contains(u.UserId))
                .ToDictionaryAsync(u => u.UserId, u => u.FullName);

            List<SettlementResponse> settlements = new();
            int i = 0, j = 0;

            while (i < creditors.Count && j < debtors.Count)
            {
                var receiver = creditors[i];
                var payer = debtors[j];

                decimal amount = Math.Min(receiver.Amount, payer.Amount);


                var settlement = _mapper.Map<SettlementResponse>(receiver);
                settlement.FromUserId = payer.UserId;
                settlement.FromUserName = userNames[payer.UserId];
                settlement.ToUserId = receiver.UserId;
                settlement.ToUserName = userNames[receiver.UserId];
                settlement.Amount = Math.Round(amount, 2);
                settlement.Summary = $"{settlement.FromUserName} pays ₹{settlement.Amount} to {settlement.ToUserName}";

                settlements.Add(settlement);

                receiver.Amount -= amount;
                payer.Amount -= amount;

                if (receiver.Amount <= 0) i++;
                if (payer.Amount <= 0) j++;
            }

            return ApiResponse<List<SettlementResponse>>.Success(Message.SettlementComplete, settlements);
        }

        public async Task<ApiResponse<bool>> DeleteSplitAsync(Guid expenseSplitId)
        {
            var split = await _expenseSplit.ExpensesSplit
                .FirstOrDefaultAsync(s => s.ExpenseSplitId == expenseSplitId);

            if (split == null)
            {
                return ApiResponse<bool>.Fail(Message.NotExpenseSplit);
            }

            _expenseSplit.ExpensesSplit.Remove(split);
            await _expenseSplit.SaveChangesAsync();

            return ApiResponse<bool>.Success(Message.DelexpenseSplit, true);
        }

    }
}

