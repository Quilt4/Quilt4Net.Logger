using System;

namespace Quilt4Net.Core
{
    internal class SessionAlreadyRegisteredException : InvalidOperationException
    {
        public SessionAlreadyRegisteredException()
            : base("The session has already been registered.")
        {
        }
    }
}