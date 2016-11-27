namespace Quilt4Net.Core.DataTransfer
{
    internal class RegisterBindingModel
    {
        internal RegisterBindingModel()
        {
        }

        public string UserName { get; internal set; }
        public string Email { get; internal set; }
        public string FullName { get; internal set; }
        public string Password { get; internal set; }
        public string ConfirmPassword { get; internal set; }
    }
}