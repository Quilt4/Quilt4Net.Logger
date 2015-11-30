﻿using System;
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

        public async Task RegisterAsync(string projectApiKey, string environment)
        {
            if (_sessionKey != Guid.Empty) throw new InvalidOperationException("The session has already been registered.");

            _sessionKey = Guid.NewGuid();

            var registerSessionRequest = new RegisterSessionRequest
            {
                SessionKey = _sessionKey,
                ProjectApiKey = projectApiKey,
                ClientStartTime = DateTime.UtcNow,
                Environment = environment,
                //Application = application,
                //Machine = machine,
                //User = user,
            };

            await _webApiClient.ExecuteCommandAsync("Session", "Register", registerSessionRequest);
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
