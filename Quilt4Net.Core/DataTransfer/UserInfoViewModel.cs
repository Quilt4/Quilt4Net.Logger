namespace Quilt4Net.Core.DataTransfer
{
    public class UserInfoViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool HasRegistered { get; set; }
        public string LoginProvider { get; set; }
    }
}