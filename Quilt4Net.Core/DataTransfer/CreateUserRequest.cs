﻿namespace Quilt4Net.Core.DataTransfer
{
    internal class RegisterBindingModel
    {
        internal RegisterBindingModel()
        {
        }

        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}