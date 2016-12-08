using System;
using System.Collections.Generic;
using Tharga.Toolkit.Console.Command;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Console.Commands
{
    internal class ProgramRootCommand : RootCommandBase
    {
        public ProgramRootCommand(IConsole console)
            : base(console, null)
        {
        }

        public override IEnumerable<HelpLine> HelpText
        {
            get { yield return new HelpLine("Quilt4 Console administration tool.", ConsoleColor.DarkMagenta); }
        }
    }
}