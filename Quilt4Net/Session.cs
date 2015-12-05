using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tharga.Quilt4Net.DataTransfer;
using Tharga.Quilt4Net.Interfaces;

namespace Tharga.Quilt4Net
{
    public class Session
    {
        private readonly IWebApiClient _webApiClient;
        private Guid _sessionKey;

        internal Session(IWebApiClient webApiClient)
        {
            _webApiClient = webApiClient;
        }

        public bool IsRegistered => _sessionKey != Guid.Empty;

        public async Task RegisterAsync(string projectApiKey, string environment)
        {
            if (_sessionKey != Guid.Empty) throw new InvalidOperationException("The session has already been registered.");

            _sessionKey = Guid.NewGuid();

            //TODO: Populate with real data
            var registerSessionRequest = new SessionData
            {
                SessionKey = _sessionKey,
                ProjectApiKey = projectApiKey,
                ClientStartTime = DateTime.UtcNow,
                Environment = environment,
                Application = new ApplicationData
                {
                    Name = "A",
                    BuildTime = null,
                    Fingerprint = "B",
                    SupportToolkitNameVersion = "C",
                    Version = "1.2.3.4",
                },
                Machine = new MachineData
                {
                    Name = "D",
                    Fingerprint = "E",
                    Data = new Dictionary<string, string>
                    {
                        { "A", "A1" },
                    }
                },
                User = new UserData
                {
                    Fingerprint = "F",
                    UserName = "G"
                },
            };

            await _webApiClient.CreateAsync("Client/Session", registerSessionRequest);
        }

        public async Task<IEnumerable<SessionData>> GetListAsync()
        {
            throw new NotImplementedException();
        }
    }
}

//using System;

//namespace Tharga.Quilt4Net
//{
//    public class Session
//    {
//        public Session(string apiKey)
//        {
//            throw new NotImplementedException();
//        }

//        public void Authorize(string token)
//        {
//            throw new NotImplementedException();
//        }

//        //public void BeginRegister()
//        //{
//        //    //Starts an unmanaged task that queues the register message, to be executed later.
//        //    throw new NotImplementedException();
//        //}

//        public SessionResponse Register()
//        {
//            //Executes and waits for the result to come back from the server
//            throw new NotImplementedException();
//        }

//        //public SessionResponse Register(TimeSpan timeout)
//        //{
//        //    //Executes and waits for the result to come back from the server
//        //    throw new NotImplementedException();
//        //}

//        //public async Task<SessionResponse> RegisterAsync()
//        //{
//        //    //Executes the register and awaits the response from the server.
//        //    throw new NotImplementedException();
//        //}

//        //public async Task<SessionResponse> RegisterAsync(TimeSpan timeout)
//        //{
//        //    //Executes the register and awaits the response from the server.
//        //    throw new NotImplementedException();
//        //}
//    }
//}
