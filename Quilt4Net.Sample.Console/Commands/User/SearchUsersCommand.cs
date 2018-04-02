using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Commands.Base;

namespace Quilt4Net.Sample.Console.Commands.User
{
    internal class SearchUsersCommand : ActionCommandBase
    {
        private readonly IQuilt4NetClient _client;

        public SearchUsersCommand(IQuilt4NetClient client)
            : base("Search", "Search for user")
        {
            _client = client;
        }

        public override void Invoke(string[] param)
        {
            var searchString = QueryParam<string>("Search string", param);
            var response = _client.Actions.User.SearchAsync(searchString).Result;
            foreach (var item in response) OutputInformation($"{item.UserName}\t{item.EMail}");
        }
    }
}