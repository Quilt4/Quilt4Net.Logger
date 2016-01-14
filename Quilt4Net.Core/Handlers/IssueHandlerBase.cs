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
            var sessionKey = await _sessionHandler.GetSessionKeyAsync();
            var issueData = PrepareIssueData(sessionKey, message, issueLevel, userHandle, data);
            return await RegisterEx(true, issueData);
        }

        public void RegisterStart(string message, MessageIssueLevel issueLevel, string userHandle = null, IDictionary<string, string> data = null)
        {
            Task.Run(async () =>
                {
                    var sessionKey = await _sessionHandler.GetSessionKeyAsync();
                    var issueData = PrepareIssueData(sessionKey, message, issueLevel, userHandle, data);
                    await RegisterEx(false, issueData);
                });
        }

        public IssueResult Register(string message, MessageIssueLevel issueLevel, string userHandle = null, IDictionary<string, string> data = null)
        {
            try
            {
                var sessionKey = _sessionHandler.GetSessionKeyAsync().Result;
                var issueData = PrepareIssueData(sessionKey, message, issueLevel, userHandle, data);
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
            var sessionKey = await _sessionHandler.GetSessionKeyAsync();
            var issueData = PrepareIssueData(sessionKey, exception, issueLevel, userHandle);
            var respnse = await RegisterEx(true, issueData);
            return respnse;
        }

        public void RegisterStart(Exception exception, ExceptionIssueLevel issueLevel = ExceptionIssueLevel.Error, string userHandle = null)
        {
            Task.Run(async () =>
                {
                    var sessionKey = await _sessionHandler.GetSessionKeyAsync();
                    var issueData = PrepareIssueData(sessionKey, exception, issueLevel, userHandle);
                    await RegisterEx(false, issueData);
                });
        }

        public IssueResult Register(Exception exception, ExceptionIssueLevel issueLevel = ExceptionIssueLevel.Error, string userHandle = null)
        {
            try
            {
                var sessionKey = _sessionHandler.GetSessionKeyAsync().Result;
                var issueData = PrepareIssueData(sessionKey, exception, issueLevel, userHandle);
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
            if (string.IsNullOrEmpty(request.SessionKey))
                throw new ArgumentException("No SessionKey has been assigned.");

            var result = new IssueResult();
            IssueResponse response = null;

            try
            {
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

        private IssueRequest PrepareIssueData(string sessionKey, Exception exception, ExceptionIssueLevel issueLevel, string userHandle)
        {
            var level = issueLevel.ToString();
            var issueType = CreateIssueTypeData(exception);

            var issueData = new IssueRequest
                                {
                                    IssueLevel = level,
                                    UserHandle = userHandle,
                                    ClientTime = DateTime.UtcNow,
                                    IssueKey = Guid.NewGuid(),
                                    IssueThreadKey = HandleIssueThreadGuid(exception),
                                    IssueType = issueType,
                                    SessionKey = sessionKey,                                    
                                };

            return issueData;
        }

        private static Dictionary<string, string> GetExceptionData(Exception exception)
        {
            return exception.Data.Cast<DictionaryEntry>().Where(x => x.Value != null).ToDictionary(item => item.Key.ToString(), item => item.Value.ToString());
        }

        private IssueTypeData CreateIssueTypeData(Exception exception)
        {
            var issueTypes = GetInnerIssueTypes(exception).ToArray();
            var exceptionData = GetExceptionData(exception);

            var issueType = new IssueTypeData
                                {
                                    Message = exception.Message,
                                    InnerIssueTypes = issueTypes,
                                    StackTrace = exception.StackTrace,
                                    Type = exception.GetType().ToString(),
                                    Data = exceptionData
                                };
            return issueType;
        }

        private IEnumerable<IssueTypeData> GetInnerIssueTypes(Exception exception)
        {
            var aggregateException = exception as AggregateException;
            if (aggregateException != null)
            {
                foreach (var inner in (aggregateException.InnerExceptions))
                {
                    yield return exception.InnerException != null ? CreateIssueTypeData(inner) : null;
                }
            }
            else
            {
                yield return exception.InnerException != null ? CreateIssueTypeData(exception.InnerException) : null;
            }
        }

        private IssueRequest PrepareIssueData(string sessionKey, string message, MessageIssueLevel issueLevel, string userHandle, IDictionary<string, string> data)
        {
            var issueType = new IssueTypeData
                                {
                                    Message = message,
                                    InnerIssueTypes = null,
                                    StackTrace = null,
                                    Type = "Message",
                                    Data = data
                                };

            var issueData = new IssueRequest
                                {
                                    UserHandle = userHandle,
                                    ClientTime = DateTime.UtcNow,
                                    IssueKey = Guid.NewGuid(),
                                    IssueThreadKey = null,
                                    IssueType = issueType,
                                    IssueLevel = issueLevel.ToString(),
                                    SessionKey = sessionKey,                                    
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