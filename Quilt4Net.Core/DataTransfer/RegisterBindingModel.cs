namespace Quilt4Net.Core.DataTransfer
{
    internal class RegisterBindingModel
    {
        internal RegisterBindingModel()
        {
        }

        public string UserName { get; internal set; }
        public string Email { get; internal set; }
        public string FirstName { get; internal set; }
        public string LastName { get; internal set; }
        public string Password { get; internal set; }
        public string ConfirmPassword { get; internal set; }
    }
}