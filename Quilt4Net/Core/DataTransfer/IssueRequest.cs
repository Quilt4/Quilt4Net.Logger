﻿using System;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core.DataTransfer
{
    public class IssueRequest : ICommand
    {
        internal IssueRequest()
        {
        }

        public Guid IssueKey { get; set; }
        public string SessionKey { get; set; }
        public DateTime ClientTime { get; set; }
        public string IssueLevel { get; set; }
        public IssueTypeData IssueType { get; set; }
        public Guid? IssueThreadKey { get; set; }
        public string UserHandle { get; set; }
    }
}