using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Commands.Base;

namespace Quilt4Net.Sample.Console.Commands.Setting
{
    internal class ListSettingsCommand : ActionCommandBase
    {
        private readonly IQuilt4NetClient _client;

        public ListSettingsCommand(IQuilt4NetClient client)
            : base("List", "List settings")
        {
            _client = client;
        }

        public override bool CanExecute(out string reasonMessage)
        {
            reasonMessage = string.Empty;
            if (!_client.Actions.User.IsAuthorized)
                reasonMessage = "Not Authorized";
            return _client.Actions.User.IsAuthorized;
        }

        public override void Invoke(string[] param)
        {
            var response = _client.Actions.ServerSetting.GetListAsync().Result;
            foreach (var item in response) OutputInformation($"{item.Name}\t{item.Value}");
        }
    }
}