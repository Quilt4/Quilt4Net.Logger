using System;
using Quilt4Net.Core.DataTransfer;

namespace Quilt4Net.Core.Events
{
    public class SessionRegisteredEventArgs : EventArgs
    {
        public SessionRegisteredEventArgs(SessionResponse response)
        {            
        }

        public string ErrorMessage
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public TimeSpan Elapsed
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsSuccess
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
