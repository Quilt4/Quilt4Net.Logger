using System;

namespace Quilt4Net
{
    public interface ISender : IDisposable
    {
        void Send(LogInput logInput);
    }
}