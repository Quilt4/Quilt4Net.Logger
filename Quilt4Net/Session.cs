using System;
using Tharga.Quilt4Net.DataTransfer;

namespace Tharga.Quilt4Net
{
    public class Session
    {
        public Session(string apiKey)
        {
            throw new NotImplementedException();
        }

        public void Authorize(string token)
        {
            throw new NotImplementedException();
        }

        //public void BeginRegister()
        //{
        //    //Starts an unmanaged task that queues the register message, to be executed later.
        //    throw new NotImplementedException();
        //}

        public SessionResponse Register()
        {
            //Executes and waits for the result to come back from the server
            throw new NotImplementedException();
        }

        //public SessionResponse Register(TimeSpan timeout)
        //{
        //    //Executes and waits for the result to come back from the server
        //    throw new NotImplementedException();
        //}

        //public async Task<SessionResponse> RegisterAsync()
        //{
        //    //Executes the register and awaits the response from the server.
        //    throw new NotImplementedException();
        //}

        //public async Task<SessionResponse> RegisterAsync(TimeSpan timeout)
        //{
        //    //Executes the register and awaits the response from the server.
        //    throw new NotImplementedException();
        //}
    }
}