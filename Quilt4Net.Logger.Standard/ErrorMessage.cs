using System;

namespace Quilt4Net
{
    public class ErrorMessage
    {
        public Guid CorrelationId { get; set; }
        public string Message { get; set; }
    }
}