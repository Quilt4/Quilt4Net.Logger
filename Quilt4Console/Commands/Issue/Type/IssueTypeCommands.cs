﻿using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Console.Commands.Issue.Type
{
    internal class IssueTypeCommands : ContainerCommandBase
    {
        public IssueTypeCommands(IIssueHandler issueHandler)
            : base("Type")
        {
            RegisterCommand(new ListIssueTypesCommand(issueHandler));
        }
    }
}