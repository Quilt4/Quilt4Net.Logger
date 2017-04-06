using System;
using System.Collections.Generic;
//using System.Net.Http;
using System.Threading.Tasks;
//using Newtonsoft.Json;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core
{
    internal class ExpectedIssues
    {
        public const int ProjectApiKeyNotSet = 1001;
        public const int UnknownType = 1002;
        public const int CannotSetProjectApiKey = 1003;
        public const int CannotSetEnvironment = 1004;
        public const int ServiceCallError = 1005;
        public const int CannotSetLocation = 1006;
        public const int CannotAutomaticallyRetrieveAssembly = 1007;
        public const int CannotChangeFirstAssembly = 1008;
        public const int FailedToExecutePost = 1009;
        public const int CannotParseIssueLevelException = 1010;
        public const int CannotParseIssueLevelMessage = 1011;
        public const int CannotSetTimeout = 1012;
        public const int Timeout = 1013;
        public const int CallTerminatedByServer = 1014;
        private readonly IConfiguration _configuration;

        private readonly Dictionary<int, string> _data = new Dictionary<int, string>();

        internal ExpectedIssues(IConfiguration configuration)
        {
            _configuration = configuration;
            _data.Add(ProjectApiKeyNotSet, "The projectApiKey has not been set.");
            _data.Add(UnknownType, "Unknown type.");
            _data.Add(CannotSetProjectApiKey, "Cannot set projectApiKey, use string.Empty instead.");
            _data.Add(CannotSetEnvironment, "Cannot set Environment to null, use string.Empty instead.");
            _data.Add(ServiceCallError, "Service call error.");
            _data.Add(CannotSetLocation, "Cannot set Location to null, use string.Empty instead.");
            _data.Add(CannotAutomaticallyRetrieveAssembly, "Cannot automatically retrieve entry assembly. Call the Session.Start method and provide the assembly to use.");
            _data.Add(CannotChangeFirstAssembly, "Cannot change first assembly.");
            _data.Add(FailedToExecutePost, "Failed to post WebAPI call. Se inner exceptions for details.");
            _data.Add(CannotParseIssueLevelException, "Cannot parse value for issue level.");
            _data.Add(CannotParseIssueLevelMessage, "Cannot parse value for issue level.");
            _data.Add(CannotSetTimeout, "Cannot set timeout to null.");
            _data.Add(Timeout, "WebAPI call timed out.");
            _data.Add(CallTerminatedByServer, "WebAPI was terminated by the server for unknown reason.");
        }

        public string GetTitle(int code)
        {
            return _data[code];
        }

        public Exception GetException(int code, Exception innerEception = null)
        {
            Exception exception;
            switch (code)
            {
                case ProjectApiKeyNotSet:
                    exception = new ProjectApiKeyNotSetException(GetMessage(code));
                    break;
                case CannotAutomaticallyRetrieveAssembly:
                case CannotChangeFirstAssembly:
                    exception = new FirstAssemblyException(GetMessage(code));
                    break;
                case CannotSetLocation:
                case CannotSetProjectApiKey:
                case CannotSetEnvironment:
                case CannotSetTimeout:
                    exception = new InvalidOperationException(GetMessage(code), innerEception);
                    break;
                case Timeout:
                    exception = new TimeoutException(GetMessage(code), innerEception);
                    break;
                case CallTerminatedByServer:
                    exception = new InvalidOperationException(GetMessage(code), innerEception);
                    break;
                default:
                    exception = new InvalidOperationException(GetMessage(code), innerEception);
                    break;
            }

            exception.HelpLink = GetHelpLink(code);
            return exception;
        }

        private string GetMessage(int code)
        {
            var message = _data[code];
            return FormatMessage(code, message);
        }

        private string FormatMessage(int code, string message)
        {
            var helpLink = GetHelpLink(code);
            var response = $"{message} Issue code #{code}. Visit '{helpLink}' for more information.";
            return response;
        }

        private string GetHelpLink(int code)
        {
            var location = _configuration.Target.Location;
            return $"{location}help/error/{code}";
        }

        //public async Task<Exception> GetExceptionFromResponse(HttpResponseMessage response)
        //{
        //    var result = await response.Content.ReadAsStringAsync();
        //    Error error;
        //    try
        //    {
        //        error = JsonConvert.DeserializeObject<Error>(result);
        //    }
        //    catch (JsonReaderException)
        //    {
        //        error = new Error();
        //    }

        //    if (error?.Type == null)
        //    {
        //        var msg = FormatMessage((int)response.StatusCode, response.ReasonPhrase + ".");
        //        var exp = new ServiceCallExcepton(msg, response, new Exception(response.ToString())) { HelpLink = GetHelpLink((int)response.StatusCode) };
        //        return exp;
        //    }

        //    var type = Type.GetType(error.Type);

        //    Exception exception;
        //    if (type == null)
        //    {
        //        return new ExpectedIssues(_configuration).GetException(ServiceCallError, new Exception(response.ToString()));
        //    }
        //    try
        //    {
        //        exception = (Exception)Activator.CreateInstance(type, "The service throw an exception. " + error.Message);
        //    }
        //    catch (Exception exp)
        //    {
        //        exception = new InvalidOperationException(error.Message, exp);
        //    }

        //    if (error.Data != null)
        //    {
        //        foreach (var data in error.Data)
        //        {
        //            exception.Data.Add(data.Key, data.Value);
        //        }
        //    }

        //    return exception;
        //}

        internal class ProjectApiKeyNotSetException : Exception
        {
            public ProjectApiKeyNotSetException(string message)
                : base(message)
            {
            }
        }

        private class FirstAssemblyException : Exception
        {
            public FirstAssemblyException(string message)
                : base(message)
            {
            }
        }

        private class Error
        {
            public string StatusCode { get; set; }
            public int Code { get; set; }
            public string Type { get; set; }
            public string Message { get; set; }
            public Dictionary<string, string> Data { get; set; }
        }
    }
}