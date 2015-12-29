namespace Quilt4Net.Core.DataTransfer
{
    public class AddRoleModel
    {
        public string UserName { get; set; }
        public string Role { get; set; }
    }

    public class ChangePasswordBindingModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class UserInfoViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool HasRegistered { get; set; }
        public string LoginProvider { get; set; }
    }
}