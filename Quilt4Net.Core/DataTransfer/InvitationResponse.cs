using System;

namespace Quilt4Net.Core.DataTransfer
{
    public class InvitationResponse
    {
        public Guid ProjectKey { get; set; }
        public string ProjectName { get; set; }
        public string InviteCode { get; set; }
        public DateTime InviteTime { get; set; }
        public string InvitedByUserName { get; set; }
    }
}