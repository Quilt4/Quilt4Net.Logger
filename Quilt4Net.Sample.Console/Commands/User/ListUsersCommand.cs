using System.Collections.Generic;
using System.Linq;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Commands.Base;

namespace Quilt4Net.Sample.Console.Commands.User
{
    internal class ListUsersCommand : ActionCommandBase
    {
        private readonly IQuilt4NetClient _client;

        public ListUsersCommand(IQuilt4NetClient client)
            : base("List", "List users")
        {
            _client = client;
        }

        public override void Invoke(string[] param)
        {
            var response = _client.Actions.User.GetListAsync().Result;

            var data = new List<string[]> { new[] { "UserName", "EMail" } };
            data.AddRange(response.Select(x => new[] { x.UserName, x.EMail }));

            OutputTable(data.ToArray());
        }
    }
}