using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Events;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core
{
    public abstract class IssueHandlerBase : IIssueHandler
    {
        private readonly object _syncRoot = new object();
        private static int _instanceCounter;
        private readonly ISessionHandler _sessionHandler;        
        private readonly List<Tuple<IssueRequest, Exception>> _issuesThatFailedToRegister = new List<Tuple<IssueRequest, Exception>>();

        protected internal IssueHandlerBase(ISessionHandler sessionHandler)
        {
            lock (_syncRoot)
            {
                if (_instanceCounter != 0)
                {
                    if (!sessionHandler.Client.Configuration.AllowMultipleInstances)
                    {
                        throw new InvalidOperationException("Multiple instances is not allowed. Set configuration setting AllowMultipleInstances to true if you want to use multiple instances of this object.");
                    }
                }
                _instanceCounter++;
            }

            _sessionHandler = sessionHandler;
        }

        public IQuilt4NetClient Client => _sessionHandler.Client;
        public event EventHandler<IssueRegistrationStartedEventArgs> IssueRegistrationStartedEvent;
        public event EventHandler<IssueRegistrationCompletedEventArgs> IssueRegistrationCompletedEvent;

        public async Task<IssueResult> RegisterAsync(string message, MessageIssueLevel issueLevel, string userHandle = null, IDictionary<string, string> data = null)
        {
            var issueData = PrepareIssueData(message, issueLevel, userHandle, data);
            return await RegisterEx(true, issueData);
        }

        public void RegisterStart(string message, MessageIssueLevel issueLevel, string userHandle = null, IDictionary<string, string> data = null)
        {
            var issueData = PrepareIssueData(message, issueLevel, userHandle, data);
            Task.Run(() => RegisterEx(false, issueData));
        }

        public IssueResult Register(string message, MessageIssueLevel issueLevel, string userHandle = null, IDictionary<string, string> data = null)
        {
            try
            {
                var issueData = PrepareIssueData(message, issueLevel, userHandle, data);
                var response = RegisterEx(true, issueData).Result;
                return response;
            }
            catch (AggregateException exception)
            {
                throw exception.InnerException;
            }
        }

        public async Task<IssueResult> RegisterAsync(Exception exception, ExceptionIssueLevel issueLevel = ExceptionIssueLevel.Error, string userHandle = null)
        {
            var issueData = PrepareIssueData(exception, issueLevel, userHandle);
            var respnse = await RegisterEx(true, issueData);
            return respnse;
        }

        public void RegisterStart(Exception exception, ExceptionIssueLevel issueLevel = ExceptionIssueLevel.Error, string userHandle = null)
        {
            var issueData = PrepareIssueData(exception, issueLevel, userHandle);
            Task.Run(() => RegisterEx(false, issueData));
        }

        public IssueResult Register(Exception exception, ExceptionIssueLevel issueLevel = ExceptionIssueLevel.Error, string userHandle = null)
        {
            try
            {
                var issueData = PrepareIssueData(exception, issueLevel, userHandle);
                var response = RegisterEx(true, issueData).Result;
                return response;
            }
            catch (AggregateException exp)
            {
                throw exp.InnerException;
            }
        }

        public async Task<IEnumerable<IssueTypeResponse>> GetIssueTypesAsync(Guid versionKey)
        {
            return await _sessionHandler.Client.WebApiClient.ExecuteQueryAsync<Guid, IEnumerable<IssueTypeResponse>>("Client/IssueType", "QueryByVersionKey", versionKey);
        }

        private async Task<IssueResult> RegisterEx(bool doThrow, IssueRequest request)
        {
            var result = new IssueResult();
            IssueResponse response = null;

            try
            {
                request.SessionToken = await _sessionHandler.GetSessionTokenAsync();

                OnIssueRegistrationStartedEvent(new IssueRegistrationStartedEventArgs(request));

                response = await _sessionHandler.Client.WebApiClient.CreateAsync<IssueRequest, IssueResponse>("Client/Issue", request);                
            }
            catch (Exception exception)
            {
                result.SetException(exception);
                _issuesThatFailedToRegister.Add(new Tuple<IssueRequest, Exception>(request, exception));

                if (doThrow)
                    throw;
            }
            finally
            {
                result.SetCompleted(response);
                OnIssueRegistrationCompletedEvent(new IssueRegistrationCompletedEventArgs(request, result));
            }

            return result;
        }

        private IssueRequest PrepareIssueData(Exception exception, ExceptionIssueLevel issueLevel, string userHandle)
        {
            var issueType = CreateIssueTypeData(exception, issueLevel);

            var issueData = new IssueRequest
            {
                Data = exception.Data.Cast<DictionaryEntry>().Where(x => x.Value != null).ToDictionary(item => item.Key.ToString(), item => item.Value.ToString()),
                UserHandle = userHandle,
                ClientTime = DateTime.UtcNow,
                IssueKey = Guid.NewGuid(),
                IssueThreadKey = HandleIssueThreadGuid(exception),
                IssueType = issueType,
            };

            return issueData;
        }

        private IssueTypeData CreateIssueTypeData(Exception exception, ExceptionIssueLevel issueLevel)
        {
            var issueType = new IssueTypeData
            {
                Message = exception.Message,
                IssueLevel = issueLevel.ToIssueLevel(_sessionHandler.Client.Configuration),
                Inner = exception.InnerException != null ? CreateIssueTypeData(exception, issueLevel) : null,
                StackTrace = exception.StackTrace,
                Type = exception.GetType().ToString(),
            };
            return issueType;
        }

        private IssueRequest PrepareIssueData(string message, MessageIssueLevel issueLevel, string userHandle, IDictionary<string, string> data)
        {            
            var issueType = new IssueTypeData
            {
                Message = message,
                IssueLevel = issueLevel.ToIssueLevel(_sessionHandler.Client.Configuration),
                Inner = null,
                StackTrace = null,
                Type = "Message",
            };

            var issueData = new IssueRequest
            {
                Data = data,
                UserHandle = userHandle,
                ClientTime = DateTime.UtcNow,
                IssueKey = Guid.NewGuid(),
                IssueThreadKey = null,
                IssueType = issueType,              
            };

            return issueData;
        }

        private static Guid HandleIssueThreadGuid(Exception exception)
        {
            var refItg = Guid.NewGuid();

            if (exception == null) return refItg;

            if (!exception.Data.Contains("IssueThreadGuid"))
            {
                exception.Data.Add("IssueThreadGuid", refItg);
            }
            else
            {
                Guid result;
                if (Guid.TryParse(exception.Data["IssueThreadGuid"].ToString(), out result))
                {
                    refItg = result;
                }
                else
                {
                    //NOTE: When there is a general message/warning event. Fire this information.
                    //Provided IssueThreadGuid cannot be parsed as Guid. Apply a new valid value.
                    exception.Data["IssueThreadGuid"] = refItg;
                }
            }

            return refItg;
        }

        protected virtual void OnIssueRegistrationStartedEvent(IssueRegistrationStartedEventArgs e)
        {
            IssueRegistrationStartedEvent?.Invoke(this, e);
        }

        protected virtual void OnIssueRegistrationCompletedEvent(IssueRegistrationCompletedEventArgs e)
        {
            IssueRegistrationCompletedEvent?.Invoke(this, e);
        }
    }
}