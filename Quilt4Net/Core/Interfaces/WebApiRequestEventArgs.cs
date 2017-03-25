using System;
using System.Diagnostics;

namespace Quilt4Net.Core.Interfaces
{
    public class WebApiRequestEventArgs : EventArgs
    {
        private readonly Stopwatch _sw;

        internal WebApiRequestEventArgs(Uri baseAddress, string path, OperationType operationType, string contentData = null)
        {
            _sw = new Stopwatch();
            _sw.Start();

            BaseAddress = baseAddress;
            Path = path;
            OperationType = operationType;
            ContentData = contentData;
        }

        public Uri BaseAddress { get; }
        public string Path { get; }
        public OperationType OperationType { get; }
        public string ContentData { get; }

        internal TimeSpan SetComplete()
        {
            _sw.Stop();
            return _sw.Elapsed;
        }
    }
}