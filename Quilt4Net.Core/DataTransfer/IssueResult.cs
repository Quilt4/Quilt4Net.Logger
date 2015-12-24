using System;
using System.Diagnostics;

namespace Quilt4Net.Core.DataTransfer
{
    public class IssueResult
    {
        private readonly Stopwatch _stopWatch;
        private Exception _exception;
        private IssueResponse _response;

        internal IssueResult()
        {
            _stopWatch = new Stopwatch();
            _stopWatch.Start();
        }

        public void SetException(Exception exception)
        {
            _exception = exception;
        }

        public void SetCompleted(IssueResponse response)
        {
            _response = response;
            _stopWatch.Stop();
        }

        public bool IsSuccess => _exception == null;
        public string ErrorMessage => _exception?.Message;
        public Exception Exception => _exception;
        public TimeSpan Elapsed => _stopWatch.Elapsed;
        public IssueResponse Response => _response;
    }
}