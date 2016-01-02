using System.Threading.Tasks;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.User
{
    internal class SearchUsersCommand : ActionCommandBase
    {
        private readonly IClient _client;

        public SearchUsersCommand(IClient client)
            : base("Search", "Search for user")
        {
            _client = client;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var index = 0;
            var searchString = QueryParam<string>("Search string", GetParam(paramList, index++));
            var response = await _client.Action.User.SearchAsync(searchString);
            foreach (var item in response)
            {
                OutputInformation("{0}\t{1}", item.UserName, item.EMail);
            }

            return true;
        }
    }
}