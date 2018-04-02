using System;
using System.Collections.Generic;
using Quilt4Net.Core;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Commands.Base;

namespace Quilt4Net.Sample.Console.Commands.Issue
{
    internal class RegisterIssueCommand : ActionCommandBase
    {
        private readonly IIssueHandler _issueHandler;

        public RegisterIssueCommand(IIssueHandler issueHandler)
            : base("Register", "Register issue")
        {
            _issueHandler = issueHandler;
        }

        public override void Invoke(string[] param)
        {
            Guid? domainKey = null;
            var dictionary = new Dictionary<string, string>
            {
                { "scanMode", "A" },
                { "resume", true.ToString() },
                { "farmName", "B" },
                { "domainKey", (domainKey ?? Guid.Empty).ToString() }
            };

            Singleton.Configuration.Instance.AllowMultipleInstances = true;
            Singleton.Issue.Instance.Register("Starting detective.", MessageIssueLevel.Information, data: dictionary);
        }
    }
}