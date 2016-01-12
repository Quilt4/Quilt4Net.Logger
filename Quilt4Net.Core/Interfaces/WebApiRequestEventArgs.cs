using System;

namespace Quilt4Net.Core.Interfaces
{
    public class WebApiRequestEventArgs : EventArgs
    {
        internal WebApiRequestEventArgs(Uri baseAddress, string path, OperationType operationType, string contentData = null)
        {
            BaseAddress = baseAddress;
            Path = path;
            OperationType = operationType;
            ContentData = contentData;
        }

        public Uri BaseAddress { get; }
        public string Path { get; }
        public OperationType OperationType { get; }
        public string ContentData { get; }
    }
}