using System;
using System.Collections.Generic;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core
{
    internal class ExpectedIssues
    {
        private readonly IConfiguration _configuration;
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

        private readonly Dictionary<int, string> _data = new Dictionary<int, string>();

        internal ExpectedIssues(IConfiguration configuration)
        {
            _configuration = configuration;
            _data.Add(ProjectApiKeyNotSet, "The client token has not been set.");
            _data.Add(UnknownType, "Unknown type.");
            _data.Add(CannotSetProjectApiKey, "Cannot set client token to null, use string.Empty instead.");
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
        }

        public string GetTitle(int code)
        {
            return _data[code];
        }

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
                default:
                    exception = new InvalidOperationException(GetMessage(code), innerEception);
                    break;
            }

            exception.HelpLink = GetHelpLink(code);
            return exception;
        }
        
        private string GetMessage(int code)
        {
            var response = $"{_data[code]} Issue code #{code}. Visit {GetHelpLink(code)} for more information";
            return response;
        }

        private string GetHelpLink(int code)
        {
            return string.Format("{1}Help/Details/{0}", code, _configuration.Target.Location);
        }
    }
}