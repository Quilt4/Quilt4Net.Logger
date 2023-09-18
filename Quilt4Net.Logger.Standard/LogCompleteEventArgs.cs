using System;

namespace Quilt4Net
{
    public class LogCompleteEventArgs : EventArgs
    {
        internal LogCompleteEventArgs(LogInput logInput, string resourceLocation, TimeSpan elapsed)
        {
            LogInput = logInput;
            ResourceLocation = resourceLocation;
            Elapsed = elapsed;
        }

        public LogInput LogInput { get; }
        public string ResourceLocation { get; }
        public TimeSpan Elapsed { get; }
    }
}