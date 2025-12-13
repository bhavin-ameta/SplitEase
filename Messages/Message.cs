namespace SplitEase.Messages
{
    public class Message
    {
        public const string EmailAlreadyExists = "Email already exists. Please use a different one.";
        public const string RegistrationSuccess = "Registration successful.";
        public const string LoginSuccess = "Login successful.";
        public const string InvalidPassword = "Invalid password.";
        public const string EmailNotFound = "Email not found. Please register first.";
        public const string Pass_Link_Gen = "Password reset link sent to your email";
        public const string InvalidToken = "Invalid Token Or Token Expire";
        public const string PasswordResetSuccess = "Password Updated successfully.";
        public const string UserNotFound = "User not found.";
        public const string UsersRetrieved = "Users retrieved successfully.";
        public const string ProfileUpdated = "Profile updated successfully.";
        public const string OldPassword = "Old password is incorrect.";
        public const string PasswordChanged = "Password changed successfully.";
        public const string UserDeleted = "User deleted successfully.";
        public const string GroupAlreadyExists = "Group Name Already Exists";
        public const string NewGroup = "Group is Created";
        public const string GroupNotFound = "Group not found";
        public const string GroupFound = "Group retrived successfully";
        public const string UpdateGroup = "Group Updated successfully";
        public const string DeleteGroup = "Group Deleted successfully";
        public const string NotMemberFound = "GroupMember Not Found";
        public const string AlreadyMember = "Member Already Exists";
        public const string MemberAdd = "GroupMember Add successfully";
        public const string GetMemberAdd = "GroupMember retrived successfully";
        public const string UpdatedMember = "GroupMember Updated successfully";
        public const string DeleteMember = "GroupMember Delete successfully";
        public const string NotMemberInGroup = "GroupMember Delete successfully";
        public const string AddExpanse = "Expense Add successfully";
        public const string GetExpense = "Expense Retrived successfully";
        public const string UpdateExpense = "Expense Update successfully";
        public const string DeleteExpense = "Expense Deleted successfully";
        public const string NotExpense = "Expense Not Found";
        public const string SplitListNotEmpty = "Split list cannot be empty";
        public const string UsersNotExist = "One or more users do not exist";
        public const string expenseSplit = "Expense split added successfully";
        public const string ExpenseEqual = "Custom split total must be equal to expense amount";
        public const string InvalidSplit = "Invalid split type! Use Equal And Custom Split";
        public const string ExpenseSplitAdd = "Expense split added successfully";
        public const string SettlementComplete = "Settlement Complete";
        public const string NotExpenseSplit = "Expense split not found";
        public const string DelexpenseSplit = "ExpenseSplit Deleted successfully";
    }
}
