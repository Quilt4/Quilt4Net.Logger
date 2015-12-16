using System;

namespace Quilt4Net.Core.DataTransfer
{
    public class SessionResponse
    {
        private Exception _exception;

        public SessionResponse()
        {
        }

        public void SetException(Exception exception)
        {
            _exception = exception;
        }

        public void SetCompleted()
        {            
        }

        public bool IsSuccess => _exception == null;
    }
}