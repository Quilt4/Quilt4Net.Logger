﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Console.Commands.User
{
    internal class ListUsersCommand : ActionCommandBase
    {
        private readonly IQuilt4NetClient _client;

        public ListUsersCommand(IQuilt4NetClient client)
            : base("List", "List users")
        {
            _client = client;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var response = await _client.Actions.User.GetListAsync();

            var data = new List<string[]> { new[] { "UserName", "EMail" } };
            data.AddRange(response.Select(x => new[] { x.UserName, x.EMail }));

            OutputTable(data.ToArray());

            return true;
        }
    }
}