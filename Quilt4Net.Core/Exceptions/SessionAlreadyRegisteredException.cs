using System;

namespace Quilt4Net.Core.Exceptions
{
    internal class SessionAlreadyRegisteredException : InvalidOperationException
    {
        public SessionAlreadyRegisteredException()
            : base("The session has already been registered.")
        {
        }
    }
}