using System;

namespace Quilt4Net.Core
{
    internal class ProjectApiKeyNotSetException : Exception
    {
        public ProjectApiKeyNotSetException(string message)
            : base(message)
        {
        }
    }
}