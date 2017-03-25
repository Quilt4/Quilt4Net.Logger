using System;

namespace Quilt4Net.Core.DataTransfer
{
    public class InviteRequest
    {
        public Guid ProjectKey { get; set; }
        public string User { get; set; }
    }
}