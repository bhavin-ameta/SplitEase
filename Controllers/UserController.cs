using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SplitEase.DTO;
using SplitEase.Messages;
using SplitEase.Services;

namespace SplitEase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;

        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserDto dto)
        {
            var response = await _userService.RegisterUser(dto);

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }


        [HttpPost("Login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginDto dto)
        {
            var response = await _userService.LoginUser(dto);

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }


        //[Authorize]
        [HttpGet("User")]
        public async Task<IActionResult> GetUser()
        {

            var response = await _userService.GetUser();

            if (!response.IsSuccess)
            {
                return NotFound(response);
            }

            return Ok(response);

        }

        [HttpGet("UserById/{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var response = await _userService.GetUserById(id);
            if (!response.IsSuccess)
            {
                return NotFound(response);
            }


            return Ok(response);
        }

        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetDto dto)
        {
            var response = await _userService.ForgetPass(dto);

            if (!response.IsSuccess)
            {
                if (response.Message == Message.EmailNotFound)
                    return NotFound(response);

                return BadRequest(response);
            }

            return Ok(response);
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] NewPassword dto)
        {
            var response = await _userService.ResetPass(dto);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        [HttpPut("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            var response = await _userService.UpdateUserProfile(dto);

            if (!response.IsSuccess)
            {
                if (response.Message == Message.UserNotFound)
                {
                    return NotFound(response);
                }

                return BadRequest(response);
            }

            return Ok(response);
        }


        [HttpPut("UpdatePassword")]

        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto dto)
        {
            var response = await _userService.UpdatePassword(dto);

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpDelete("DeleteUser")]

        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _userService.DeleteUser(id);
            if (!user.IsSuccess)
            {
                return NotFound(user);
            }
            return Ok(user);
        }

    }

    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpPost("NewGroup")]
        public async Task<IActionResult> AddGroup([FromBody] GroupDto group)
        {
            var data = await _groupService.AddGroupAsync(group);
            if (!data.IsSuccess)
            {
                return BadRequest(data);
            }

            //var response = data.Data!;

            return Ok(data);

        }


        [HttpGet("Group")]
        public async Task<IActionResult> GetGroup()
        {
            var data = await _groupService.GetGroupAsync();
            if (!data.IsSuccess)
            {
                return NotFound(data);
            }
            return Ok(data);
        }


        [HttpGet("GroupById/{id}")]
        public async Task<IActionResult> GetGroupById(Guid id)
        {
            var group = await _groupService.GetGroupByIdAsync(id);
            if (!group.IsSuccess)
            {
                return NotFound(group);
            }
            return Ok(group);
        }

        [HttpPut("UpdateGroup/{id}")]
        public async Task<IActionResult> UpdateGroup(Guid id, UpdateGroup dto)
        {
            var UpdateGroup = await _groupService.UpdateGroupAsync(id, dto);
            if (!UpdateGroup.IsSuccess)
            {
                return NotFound(UpdateGroup);
            }

            return Ok(UpdateGroup);


        }

        [HttpDelete("DeleteGroup/{id}")]
        public async Task<IActionResult> DeleteGroup(Guid id)
        {
            var DeleteGroup = await _groupService.DeleteGroupAsync(id);
            if (!DeleteGroup.IsSuccess)
            {
                return NotFound(DeleteGroup);
            }

            return Ok(DeleteGroup);
        }


    }

    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpPost("AddMember")]
        public async Task<IActionResult> AddMember([FromBody] AddGroupMemberDto dto)
        {
            var response = await _memberService.AddMemberAsync(dto);

            if (!response.IsSuccess)
            {
                if (response.Message == Message.GroupNotFound ||
                    response.Message == Message.UserNotFound)
                {
                    return NotFound(response);
                }

                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("GetMember")]
        public async Task<IActionResult> GetMember()
        {
            var getGroup = await _memberService.GetMemberAsync();
            if (!getGroup.IsSuccess)
            {
                return NotFound();
            }

            return Ok(getGroup);
        }

        [HttpGet("GetMemberById/{id}")]
        public async Task<IActionResult> GetMemberById(Guid id)
        {
            var member = await _memberService.GetMemberByIdAsync(id);
            if (!member.IsSuccess)
            {
                return NotFound();
            }

            return Ok(member);
        }

        [HttpPut("UpdateMember/{id}")]
        public async Task<IActionResult> UpdateMember(Guid id, MemberUpdatedDto dto)
        {
            var member = await _memberService.UpdateGroupMemberAsync(id, dto);
            if (!member.IsSuccess)
            {
                return NotFound(member);
            }
            return Ok(member);


        }

        [HttpDelete("DeleteMember")]
        public async Task<IActionResult> DeleteMemeber(Guid id)
        {
            var DeleteMember = await _memberService.DeleteMemberAsync(id);
            if (!DeleteMember.IsSuccess)
            {
                return NotFound(DeleteMember);
            }
            return Ok(DeleteMember);
        }

    }
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpensiveService _expensiveService;
        public ExpenseController(IExpensiveService expensiveService)
        {
            _expensiveService = expensiveService;
        }

        [HttpPost("AddExpense")]
        public async Task<IActionResult> AddExpense([FromBody] AddExpenseDto dto)
        {
            var response = await _expensiveService.AddExpenseAsync(dto);

            if (!response.IsSuccess)
            {
                if (response.Message == Message.UserNotFound ||
                    response.Message == Message.GroupNotFound)
                    return NotFound(response);

                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("Expense")]

        public async Task<IActionResult> GetExpense()
        {
            var expenseGroup = await _expensiveService.GetExpenseAsync();
            if (!expenseGroup.IsSuccess)
            {
                return NotFound(expenseGroup);
            }

            return Ok(expenseGroup);
        }

        [HttpGet("Expense/{id}")]

        public async Task<IActionResult> GetExpense(Guid id)
        {
            var ExpenseGroup = await _expensiveService.GetExpenseByIdAsync(id);
            if (!ExpenseGroup.IsSuccess)
            {
                return NotFound(ExpenseGroup);
            }
            return Ok(ExpenseGroup);
        }

        [HttpPut("UpdateExpense/{id}")]
        public async Task<IActionResult> UpdateExpense(Guid id, UpdateExpenseDto dto)
        {
            var Expense = await _expensiveService.UpdateExpenseAsync(id, dto);
            if (!Expense.IsSuccess)
            {
                return NotFound(Expense);
            }

            return Ok(Expense);
        }

        [HttpDelete("DeleteExpense")]
        public async Task<IActionResult> DeleteExpense(Guid id)
        {
            var expensedel = await _expensiveService.DeleteExpenseASync(id);
            if (!expensedel.IsSuccess)
            {
                return NotFound(expensedel);
            }

            return Ok(expensedel);

        }



    }
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseSplitController : ControllerBase
    {
        private readonly IExpensiveSplitService _expensiveSplitService;

        public ExpenseSplitController(IExpensiveSplitService expensiveSplitService)
        {
            _expensiveSplitService = expensiveSplitService;
        }

        [HttpPost("AddExpenseSplit")]
        public async Task<IActionResult> AddExpenseSplit([FromBody] AddExpenseSplitDto dto)
        {
            var response = await _expensiveSplitService.AddExpenseSplitAsync(dto);
            if (!response.IsSuccess)
            {
                if (response.Message == Message.NotExpense ||
                response.Message == Message.GroupNotFound)
                {
                    return NotFound(response);
                }


                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("{groupId}")]
        public async Task<IActionResult> GetSettlement(Guid groupId)
        {
            if (groupId == Guid.Empty)
                return BadRequest("Invalid Group Id");

            var response = await _expensiveSplitService.CalculateSettlementAsync(groupId);

            if (!response.IsSuccess)
            {
                return NotFound(response);

            }
            return Ok(response);
        }

        [HttpDelete("{expenseSplitId}")]
        public async Task<IActionResult> DelSplitWise(Guid expenseSplitId)
        {
            var response = await _expensiveSplitService.DeleteSplitAsync(expenseSplitId);
            if (!response.IsSuccess)
            {
                return NotFound(response);

            }

            return Ok(response);
        }
    }
}




